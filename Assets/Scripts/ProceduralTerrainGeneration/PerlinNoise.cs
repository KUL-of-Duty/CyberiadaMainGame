using UnityEngine;
using System;
using System.Collections;

public static class PerlinNoise
{
    public static float[,] GenerateMap(int mapWitdth, int mapHeight, float scale, int seed, int octaves, float lucunarity, float persistance){
        float[,]noiseMap = new float[mapWitdth, mapHeight];

        System.Random prng = new System.Random(seed);


        float minValue = float.MaxValue;
        float maxValue = float.MinValue;


        Vector2[] octaveOffsets = new Vector2[octaves];
        for(int i = 0; i < octaves; i++){
            float offsetX = prng.Next(-100000, 100000);
            float offsetY = prng.Next(-100000, 100000);
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }


        for(int x = 0; x < mapWitdth; x++)
            for(int y = 0; y < mapHeight; y++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for(int oct = 0; oct < octaves; oct++){
                float pixelX = (x / scale) * frequency + octaveOffsets[oct].x;
                float pixelY = (y / scale) * frequency + octaveOffsets[oct].y;

                float perlinValue = Mathf.PerlinNoise(pixelX, pixelY) * 2 - 1;
                noiseHeight += perlinValue * amplitude;

                frequency *= lucunarity;
                amplitude *= persistance;
                }
                if(noiseHeight > maxValue) maxValue = noiseHeight;
                else if(noiseHeight < minValue) minValue = noiseHeight;
                noiseMap[x,y] = noiseHeight;
            }
            for(int x = 0; x < mapWitdth; x++)
                for(int y = 0; y < mapHeight; y++)
                {
                    noiseMap[x,y] = Mathf.InverseLerp(minValue, maxValue, noiseMap[x,y]);
                }
    
    return noiseMap;
    }   
} 
