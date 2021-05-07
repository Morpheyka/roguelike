using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public static class MeshGenerator
{
    private const int CHUNK_SIZE = 65535;

    public static Mesh GenerateTerrainMesh(float[,] heightMap, AnimationCurve heightCurve, float seaLevel,
        float heightMultiplier)
    {
        var width = heightMap.GetLength(0);
        var height = heightMap.GetLength(1);

        var topLeftX = (width - 1) * -0.5f;
        var topLeftZ = (height - 1) * 0.5f;

        var terrain = new MeshData(width, height);
        var vertexIndex = 0;

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var heightAtPoint = heightMap[x, y];
                var vertexHeight = heightAtPoint <= seaLevel
                    ? 0
                    : heightCurve.Evaluate(Mathf.InverseLerp(seaLevel, 1f, heightAtPoint)) *
                      heightMultiplier;

                terrain.vertices[vertexIndex] = new Vector3(topLeftX + x, vertexHeight, topLeftZ - y);
                terrain.uvs[vertexIndex] = new Vector2(x / (float) width, y / (float) height);

                if (x < width - 1 && y < height - 1)
                {
                    terrain.AddTriangle(vertexIndex, vertexIndex + width + 1, vertexIndex + width);
                    terrain.AddTriangle(vertexIndex + width + 1, vertexIndex, vertexIndex + 1);
                }

                vertexIndex++;
            }
        }

        return terrain.CreateMesh();
    }

    private struct MeshData
    {
        public readonly Vector3[] vertices;
        private readonly int[] triangles;
        public readonly Vector2[] uvs;

        private int triangleIndex;

        public MeshData(int width, int height)
        {
            vertices = new Vector3[width * height];
            uvs = new Vector2[width * height];
            triangles = new int[(width - 1) * (height - 1) * 6];
            triangleIndex = 0;
        }

        public void AddTriangle(int a, int b, int c)
        {
            triangles[triangleIndex] = a;
            triangles[triangleIndex + 1] = b;
            triangles[triangleIndex + 2] = c;
            triangleIndex += 3;
        }

        public Mesh CreateMesh()
        {
            var mesh = new Mesh
            {
                vertices = vertices,
                indexFormat = vertices.Length > CHUNK_SIZE ? IndexFormat.UInt32 : IndexFormat.UInt16,
                subMeshCount = Mathf.CeilToInt((float) triangles.Length / CHUNK_SIZE),
                uv = uvs,
            };

            var subMeshTriangles = triangles.ToList();

            for (var i = 0; i < mesh.subMeshCount; i++)
            {
                var subMeshTrianglesCount = subMeshTriangles.Count < CHUNK_SIZE ? subMeshTriangles.Count : CHUNK_SIZE;
                var targetTriangles = subMeshTriangles.GetRange(0, subMeshTrianglesCount).ToArray();

                mesh.SetTriangles(targetTriangles, i);
                subMeshTriangles.RemoveRange(0, subMeshTrianglesCount);
            }

            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            mesh.Optimize();

            return mesh;
        }
    }
}