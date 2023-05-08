using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class TerrainGenerator : MonoBehaviour
{

    [SerializeField]
    private GameObject _terrainPrefab;

    private void OnEnable()
    {
        var mesh = new Mesh
        {
            name = "Terrain Mesh"
        };

        Vector3[] vertices = new Vector3[9];

        for(int x = 0; x < 1; x++)
        {
            for (int y = 0; y < 1; y++)
            {
                vertices = CreateOctogonTile(x, y);
            }
        }

        int[] triangles = new int[8 * 3];
        int index = 0;


        for (int k = 0; k < 8; k++)
        {
            triangles[index++] = 8;
            triangles[index++] = (k + 1 > 7) ? 0 : k + 1;
            triangles[index++] = k;
        }

        Vector3[] normals = new Vector3[vertices.Length];

        for (int i = 0; i < vertices.Length; i++)
        {
            normals[i] = Vector3.up;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;


        _terrainPrefab.GetComponent<MeshFilter>().mesh = mesh;
    }

    private Vector3[] CreateOctogonTile(int x, int y)
    {
        Vector3[] temp = new Vector3[9];
        temp[0] = new Vector3(x - 1.5f, 0, y + 0.5f);
        temp[1] = new Vector3(x - 1.5f, 0, y - 0.5f);
        temp[2] = new Vector3(x - .5f, 0, y - 1.5f);
        temp[3] = new Vector3(x + .5f, 0, y - 1.5f);
        temp[4] = new Vector3(x + 1.5f, 0, y - 0.5f);
        temp[5] = new Vector3(x + 1.5f, 0, y + 0.5f);
        temp[6] = new Vector3(x + .5f, 0, y + 1.5f);
        temp[7] = new Vector3(x - .5f, 0, y + 1.5f);
        temp[8] = new Vector3(x, 0, y);

        return temp;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
