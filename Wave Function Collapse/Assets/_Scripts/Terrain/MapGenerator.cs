using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Scripts.Terrain
{
    public class MapGenenerator : MonoBehaviour
    {

        public Vector3Int map;
        public float noiseScale;

        public int octave;
        [Range(0, 1)]
        public float persistance;
        public float lacunarity;

        public int seed;
        public Vector3 offset;

        public bool autoUpdate;

        public void GenerateMap()
        {
            float[,,] noiseMap = Noise.GenerateNoiseMap(map, noiseScale, octave, persistance, lacunarity, seed, offset);

            //MapDisplay display = FindObjectOfType<MapDisplay>();

            //display.DrawNoiseMap(noiseMap);

            MeshGenerator meshGen = FindObjectOfType<MeshGenerator>();

            meshGen.GenerateMeshMarchingCube(noiseMap);
        }

        private void OnValidate()
        {
            if (lacunarity < 1)
            {
                lacunarity = 1;
            }
            if (octave < 1)
            {
                octave = 1;
            }
        }
    }
}
