using System.Collections.Generic;
using UnityEngine;

public class MapGen : MonoBehaviour
{
    const int threadGroupSize = 8;
    
    public GameObject plane;

    [Header ( "Map Settings")]
    public ComputeShader densityGenerator;

    public bool fixedMapSize;
    public Vector3Int ChunkSize = Vector3Int.one;
    public Transform viewer;
    private Vector3Int lastViewerPos;
    public float viewDistance = 30;

    [Space()]
    public ComputeShader computeMarch;
    public Material mat;
    public bool generateColliders = false;

    [Header("Voxel Settings")]
    public float isoLevel = .1f;
    public float boundsSize = 10;
    public Vector3 offset = Vector3.zero;
    
    [Range(2, 100)]
    public int numPointsPerAxis = 30;
    
    GameObject chunkHolder;
    const string chunkHolderName = "Chunks Holder";
    private Queue<Chunk> recycleableChunks;
    private List<Chunk> chunks;
    private Dictionary<Vector3Int, Chunk> existingChunks;

    private ComputeBuffer vertexBuffer;
    private ComputeBuffer trianglesBuffer;
    private ComputeBuffer triCountBuffer;

    public bool settingsUpdated;

    public bool autoUpdate;

    // Start is called before the first frame update
    void Awake()
    {
        if (Application.isPlaying && !fixedMapSize)
        {
            InitVariableChunkStructures();

            var oldChunks = FindObjectsOfType<Chunk>();
            for (int i = oldChunks.Length - 1; i >= 0; i--)
            {
                Destroy(oldChunks[i].gameObject);
            }
        }
    }

    void InitVariableChunkStructures()
    {
        recycleableChunks = new Queue<Chunk>();
        chunks = new List<Chunk>();
        existingChunks = new Dictionary<Vector3Int, Chunk>();
    }

    private void InitBuffer()
    {
        int numPoints = numPointsPerAxis * numPointsPerAxis*2 * numPointsPerAxis;
        int numVoxelsPerAxis = numPointsPerAxis - 1;
        int numVoxelsAxisY = (numPointsPerAxis * 2) - 1;
        int numVoxels = numVoxelsPerAxis * numVoxelsAxisY * numVoxelsPerAxis;
        int maxTriangleCount = numVoxels * 5;

        if (vertexBuffer != null || trianglesBuffer != null || triCountBuffer != null)
        {
            ReleaseBuffers();
        }

        trianglesBuffer = new ComputeBuffer(maxTriangleCount, sizeof(float) * 3 * 3, ComputeBufferType.Append);
        vertexBuffer = new ComputeBuffer(numPoints, sizeof(float) * 4);
        triCountBuffer = new ComputeBuffer(1, sizeof(int), ComputeBufferType.Raw);
    }

    void ReleaseBuffers()
    {
        if (trianglesBuffer != null)
        {
            trianglesBuffer.Release();
            vertexBuffer.Release();
            triCountBuffer.Release();
        }
    }

    private void Start()
    {
        CreateChunkHolder();
    }

    void CreateChunkHolder()
    {
        if (chunkHolder == null)
        {
            if (GameObject.Find(chunkHolderName))
            {
                chunkHolder = GameObject.Find(chunkHolderName);
            }
            else
            {
                chunkHolder = new GameObject(chunkHolderName);
            }
        }
    }

    void Update()
    {
        if (lastViewerPos == null || lastViewerPos != new Vector3Int((int)viewer.position.x, (int)viewer.position.y, (int)viewer.position.z) || settingsUpdated)
        {
            InitBuffer();

            lastViewerPos = new Vector3Int((int)viewer.position.x, (int)viewer.position.y, (int)viewer.position.z);
            RequestMeshUpdate();
        }

        if (Application.isPlaying)
        {
            ReleaseBuffers();
        }
    }

    void RequestMeshUpdate()
    {
        Vector3 v = viewer.position;


        for (int i = chunks.Count-1; i >= 0; i--)
        {
            Chunk chunk = chunks[i];

            if (Vector3.Distance(v, chunk.position) < viewDistance)
            {
                existingChunks.Remove(chunk.coord);
                recycleableChunks.Enqueue(chunk);
                chunks.RemoveAt(i);
            }
        }

        int maxChunksInView = Mathf.CeilToInt(viewDistance / boundsSize);

        for (int x = -maxChunksInView; x < maxChunksInView; x++)
        {
            for (int z = -maxChunksInView; z < maxChunksInView; z++)
            {
                Vector3Int coord = new Vector3Int(x, 0, z) + new Vector3Int(1,0,1) * lastViewerPos;

                if (existingChunks.ContainsKey(coord))
                {
                    continue;
                }

                if (recycleableChunks.Count > 0)
                {
                    Chunk chunk = recycleableChunks.Dequeue();
                    chunk.coord = coord;
                    existingChunks.Add(coord, chunk);
                    chunks.Add(chunk);
                    UpdateChunkMesh(chunk);
                }
                else
                {
                    Chunk chunk = CreateChunk(coord);
                    chunk.coord = coord;
                    chunk.SetUp(mat, generateColliders);
                    existingChunks.Add(coord, chunk);
                    chunks.Add(chunk);
                    UpdateChunkMesh(chunk);
                }
            }
        }
    }

    Chunk CreateChunk(Vector3Int chunkPos)
    {
        Vector3 position = new Vector3(chunkPos.x, 0, chunkPos.z);

        GameObject chunk = new GameObject("Chunk " + "(" + chunkPos.x + ", " + chunkPos.y + ", " + chunkPos.z + ")");
        chunk.transform.parent = chunkHolder.transform;

        Chunk newChunk = chunk.AddComponent<Chunk>();
        newChunk.coord = chunkPos;

        return newChunk;
    }

    void UpdateChunkMesh(Chunk chunk)
    {
        float pointSpacing = boundsSize / (float)(numPointsPerAxis - 1);

        densityGenerator.SetBuffer(0, "vertex", vertexBuffer);
        densityGenerator.SetInt("numPointsPerAxis", numPointsPerAxis);
        densityGenerator.SetVector("centre", new Vector3(chunk.coord.x,0,chunk.coord.z) * boundsSize);
        densityGenerator.SetFloat("spacing", pointSpacing);
        densityGenerator.SetFloat("boundsSize", boundsSize);

        densityGenerator.Dispatch(0, numPointsPerAxis, numPointsPerAxis*2, numPointsPerAxis);

        int numVoxelsPerAxis = numPointsPerAxis - 1;
        int numVoxelsAxisY = (numPointsPerAxis * 2) - 1;
        int numThreadsPerAxis = Mathf.CeilToInt(numVoxelsPerAxis / (float)threadGroupSize);
        int numThreadsAxisY = Mathf.CeilToInt(numVoxelsAxisY / (float)threadGroupSize);

        trianglesBuffer.SetCounterValue(0);

        computeMarch.SetBuffer(0, "vertex", vertexBuffer);
        computeMarch.SetBuffer(0, "triangles", trianglesBuffer);
        computeMarch.SetInt("numPointsPerAxis", numPointsPerAxis);
        computeMarch.SetFloat("isoLevel", isoLevel);
        computeMarch.Dispatch(0, numThreadsPerAxis, numThreadsAxisY, numThreadsPerAxis);

        ComputeBuffer.CopyCount(trianglesBuffer, triCountBuffer, 0);
        int[] triCountArray = { 0 };
        triCountBuffer.GetData(triCountArray);
        int numTris = triCountArray[0];

        Triangle[] trianglesArray = new Triangle[numTris];
        trianglesBuffer.GetData(trianglesArray, 0, 0, numTris);

        chunk.mesh.Clear();

        Vector3[] v = new Vector3[numTris * 3];
        int[] meshTriangles = new int[numTris * 3];

        for (int i = 0; i < numTris; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                meshTriangles[i * 3 + j] = i * 3 + j;
                v[i * 3 + j] = trianglesArray[i][j];
            }
        }

        chunk.mesh.vertices = v;
        chunk.mesh.triangles = meshTriangles;

        chunk.mesh.RecalculateNormals();
    }

    public void OnGenerateMap()
    {
        InitBuffer();
        UpdateAllChunks();
        ReleaseBuffers();
    }

    public void UpdateAllChunks()
    {

        // Create mesh for each chunk
        foreach (Chunk chunk in chunks)
        {
            UpdateChunkMesh(chunk);
        }

    }

    void OnDestroy()
    {
        if (Application.isPlaying)
        {
            ReleaseBuffers();
        }
    }
}

public struct Triangle
{
    public Vector3 a;
    public Vector3 b;
    public Vector3 c;

    public Vector3 this[int i]
    {
        get
        {
            switch (i)
            {
                case 0:
                    return a;
                case 1:
                    return b;
                default:
                    return c;
            }
        }
    }
};