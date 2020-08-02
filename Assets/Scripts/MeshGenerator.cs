using UnityEngine;

public static class MeshGenerator
{
    public static MeshData GenerateWorldMeshData(WorldTile[,] tiles, Vector2Int size, float heightMultiplier, AnimationCurve heightCurve)
    {
        int width = size.x;
        int height = size.y;
        float topLeftX = (width - 1) * -0.5f;
        float topLeftZ = (height - 1) * 0.5f;

        MeshData data = new MeshData(width, height);
        int vertexIndex = 0;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                data.Vericles[vertexIndex] = new Vector3(topLeftX + x, heightCurve.Evaluate(tiles[x, y].Height) * heightMultiplier, topLeftZ - y);
                data.Uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);

                if(x < width - 1 && y < height - 1)
                {
                    data.AddTriangle(vertexIndex, vertexIndex + width + 1, vertexIndex + width);
                    data.AddTriangle(vertexIndex + width + 1, vertexIndex, vertexIndex + 1);
                }

                vertexIndex++;
            }
        }

        return data;
    }

    public class MeshData
    {
        public Vector3[] Vericles { get; private set; }
        public int[] Triangles { get; private set; }
        public Vector2[] Uvs { get; private set; }

        private int _trianglesIndex = 0;

        public MeshData(int widht, int height)
        {
            Vericles = new Vector3[widht * height];
            Uvs = new Vector2[widht * height];
            Triangles = new int[(widht - 1) * (height - 1) * 6];
        }

        public Mesh Generate()
        {
            Mesh target = new Mesh
            {
                vertices = Vericles,
                triangles = Triangles,
                uv = Uvs
            };

            target.RecalculateNormals();

            return target;
        }

        public void AddTriangle(int a, int b, int c)
        {
            Triangles[_trianglesIndex] = a;
            Triangles[_trianglesIndex + 1] = b;
            Triangles[_trianglesIndex + 2] = c;

            _trianglesIndex += 3;
        }
    }
}
