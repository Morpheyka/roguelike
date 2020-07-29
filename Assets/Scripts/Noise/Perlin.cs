using UnityEngine;

public class Perlin
{
    public float[,] Generate(Vector2Int size, Perlin.Data data, int seed)
    {
        float[,] noise = new float[size.x, size.y];

        System.Random prnd = new System.Random(seed);
        Vector2[] octavesOffset = new Vector2[data.Octaves];

        for (int i = 0; i < data.Octaves; i++)
        {
            float offsetX = prnd.Next(-100000, 100000);
            float offsetY = prnd.Next(-100000, 100000);

            octavesOffset[i] = new Vector2(offsetX, offsetY);
        }

        if (data.Scale <= 0f)
            data.Scale = 0.001f;

        float minNoiseHeight = float.MaxValue;
        float maxNoiseHeight = float.MinValue;

        float halfWidht = size.x* 0.5f;
        float halfHeight = size.y * 0.5f;

        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                float amplitute = 1f;
                float frequence = 1f;
                float noiseHeight = 0f;

                for (int i = 0; i < data.Octaves; i++)
                {
                    float nx = (x - halfWidht) / data.Scale * frequence + octavesOffset[i].x;
                    float ny = (y - halfHeight) / data.Scale * frequence + octavesOffset[i].y;

                    float perlinValue = Mathf.PerlinNoise(nx, ny) * 2f - 1f;
                    noiseHeight += perlinValue * amplitute;

                    amplitute *= data.Persistance;
                    frequence *= data.Lacunarity;
                }

                if (noiseHeight > maxNoiseHeight)
                    maxNoiseHeight = noiseHeight;
                else if (noiseHeight < minNoiseHeight)
                    minNoiseHeight = noiseHeight;

                noise[x, y] = noiseHeight;
            }
        }

        for (int y = 0; y < size.y; y++)
            for (int x = 0; x < size.x; x++)
                noise[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noise[x, y]);

        return noise;
    }

    [System.Serializable]
    public class Data
    {
        [Range(1.1f, 100f)] public float Scale = 1.1f;
        [Range(0, 100)] public int Octaves = 1;
        [Range(0.001f, 1f)] public float Persistance = 1f;
        [Range(0.001f, 100f)] public float Lacunarity = 1f;
    }
}