using UnityEngine;

public class Perlin
{
    public float[,] Generate(Vector2Int size, Perlin.Data data, int seed)
    {
        float[,] noise = new float[size.x, size.y];

        System.Random prnd = new System.Random(seed);
        Vector2[] octavesOffset = new Vector2[data.octaves];

        for (int i = 0; i < data.octaves; i++)
        {
            float offsetX = prnd.Next(-100000, 100000);
            float offsetY = prnd.Next(-100000, 100000);

            octavesOffset[i] = new Vector2(offsetX, offsetY);
        }

        if (data.scale <= 0f)
            data.scale = 0.001f;

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

                for (int i = 0; i < data.octaves; i++)
                {
                    float nx = (x - halfWidht) / data.scale * frequence + octavesOffset[i].x;
                    float ny = (y - halfHeight) / data.scale * frequence + octavesOffset[i].y;

                    float perlinValue = Mathf.PerlinNoise(nx, ny) * 2f - 1f;
                    noiseHeight += perlinValue * amplitute;

                    amplitute *= data.persistance;
                    frequence *= data.lacunarity;
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
        [Range(1.1f, 100f)] public float scale = 25f;
        [Range(0, 100)] public int octaves = 5;
        [Range(0.001f, 1f)] public float persistance = 0.5f;
        [Range(0.001f, 100f)] public float lacunarity = 2f;
        public bool useFalloff = true;
    }
}