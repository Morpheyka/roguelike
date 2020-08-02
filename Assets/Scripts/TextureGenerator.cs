using UnityEngine;

public static class TextureGenerator
{
    public static Texture2D CreateHightMapTexture(WorldTile[,] map, Color lower, Color hight)
    {
        int width = map.GetLength(0);
        int height = map.GetLength(1);

        Texture2D texture = new Texture2D(width, height)
        {
            filterMode = FilterMode.Point,
            wrapMode = TextureWrapMode.Clamp
        };

        Color[] colourMap = new Color[width * height];

        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                colourMap[y * width + x] = Color.Lerp(lower, hight, map[x, y].Height);

        texture.SetPixels(colourMap);
        texture.Apply();

        return texture;
    }

    public static Texture2D CreateColourHightMapTexture(Vector2Int size, Color[] colours)
    {
        Texture2D texture = new Texture2D(size.x, size.y)
        {
            filterMode = FilterMode.Point,
            wrapMode = TextureWrapMode.Clamp
        };

        texture.SetPixels(colours);
        texture.Apply();

        return texture;
    }
}