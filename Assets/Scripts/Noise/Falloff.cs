using UnityEngine;

public class Falloff
{
    public float[,] Generate(Vector2Int size)
    {
        float[,] map = new float[size.x, size.y];

        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                float nx = x / (float)size.x * 2f - 1f;
                float ny = y / (float)size.y * 2f - 1f;

                float value = Mathf.Max(Mathf.Abs(nx), Mathf.Abs(ny));
                map[x, y] = Evalute(value);
            }
        }

        return map;
    }

    private float Evalute(float value)
    {
        float a = 3f;
        float b = 5f;

        return Mathf.Pow(value, a) / (Mathf.Pow(value, a) + Mathf.Pow(b - b * value, a));
    }
}