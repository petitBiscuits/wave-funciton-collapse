using System.Collections;
using UnityEngine;

namespace Assets._Scripts.Terrain
{
    public static class Noise
    {
        public static float[,,] GenerateNoiseMapV1(int mapX, int mapY, int mapZ, float scale, int octave, float persistance, float lacunarity, int seed, Vector2 offset)
        {
            // float[,,] noiseMap = new float[mapX, mapY, mapZ];

            // System.Random prng = new System.Random(seed);
            // Vector2[] octaveOffSets = new Vector2[octave];
            // for (int i = 0; i < octave; i++)
            // {
            //     float offSetX = prng.Next(-100000, 100000) + offset.x;
            //     float offSetY = prng.Next(-100000, 100000) + offset.y;
            //     octaveOffSets[i] = new Vector2(offSetX, offSetY);
            // }

            // if (scale <= 0)
            // {
            //     scale = 0.0001f;
            // }

            // float maxNoiseHeight = float.MinValue;
            // float minNoiseHeight = float.MaxValue;

            // float halfWidth = mapX / 2f;
            // float halfHeight = mapY / 2f;

            // for (int y = 0; y < mapY; y++)
            // {
            //     for (int x = 0; x < mapX; x++)
            //     {
            //         float amplitude = 1;
            //         float frequency = 1;
            //         float noiseHeight = 0;

            //         for (int i = 0; i < octave; i++)
            //         {
            //             float sampleX = (x - halfWidth) / scale * frequency + octaveOffSets[i].x;
            //             float sampleY = (y - halfHeight) / scale * frequency + octaveOffSets[i].y;

            //             float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
            //             noiseHeight += perlinValue * amplitude;

            //             amplitude *= persistance;
            //             frequency *= lacunarity;
            //         }

            //         if (noiseHeight > maxNoiseHeight)
            //         {
            //             maxNoiseHeight = noiseHeight;
            //         }
            //         else if (noiseHeight < minNoiseHeight)
            //         {
            //             minNoiseHeight = noiseHeight;
            //         }

            //         noiseMap[x, y] = noiseHeight;
            //     }
            // }

            // for (int y = 0; y < mapY; y++)
            // {
            //     for (int x = 0; x < mapX; x++)
            //     {
            //         noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            //     }
            // }

            //return noiseMap;
            return null;
        }

        public static float[,,] GenerateNoiseMap(Vector3Int map, float scale, int octave, float persistance, float lacunarity, int seed, Vector3 offset)
        {
            FastNoiseLite noise = new FastNoiseLite(seed);
            noise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
            noise.SetFrequency(0.05f);
            noise.SetFractalOctaves(octave);
            noise.SetFractalLacunarity(lacunarity);
            noise.SetFractalGain(persistance);

            int width = map.y;
            int height = map.x;
            int depth = map.z;

            float[,,] noiseValues = new float[width, height, depth];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int z = 0; z < depth; z++)
                    {
                        float noiseValue = noise.GetNoise(x+offset.x, y+offset.y, z+offset.z);
                        noiseValues[x, y, z] = noiseValue;
                    }
                }
            }
            return noiseValues;
        }
    }
}