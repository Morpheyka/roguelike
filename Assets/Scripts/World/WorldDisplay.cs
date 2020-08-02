using UnityEngine;

public class WorldDisplay : MonoBehaviour
{   
    [Header("Prefabs")]
    [SerializeField] private GameObject _meshPrefab = null;
    [Header("Properties")]
    [SerializeField] private TerrainConfig _config = null;
    [SerializeField] private float _heightMultiplier = 1f;
    [SerializeField] private AnimationCurve _heightCurve = null;

    private GameObject _targetDisplay = null;
    private MeshRenderer _meshRender = null;
    private MeshFilter _meshFilter = null;

    public void Draw(WorldTile[,] worldTiles, Vector2Int worldSize)
    {
        Prepare();
        DrawMesh(worldTiles, worldSize);
    }

    private void Prepare()
    {
        if (_targetDisplay != null)
            Destroy(_targetDisplay.gameObject);

        _targetDisplay = Instantiate(_meshPrefab, transform);
        _meshRender = _targetDisplay.GetComponent<MeshRenderer>();
        _meshFilter = _targetDisplay.GetComponent<MeshFilter>();
    }

    private void DrawMesh(WorldTile[,] tiles, Vector2Int size)
    {
        Color[] colourMap = GenerateColourMap(tiles, size);
        _meshRender.sharedMaterial.mainTexture = TextureGenerator.CreateColourHightMapTexture(size, colourMap);

        MeshGenerator.MeshData meshData = MeshGenerator.GenerateWorldMeshData(tiles, size, _heightMultiplier, _heightCurve);
        _meshFilter.sharedMesh = meshData.Generate();
    }

    private Color[] GenerateColourMap(WorldTile[,] tiles, Vector2Int size)
    {
        int width = size.x;
        int height = size.y;

        Color[] colourMap = new Color[width * height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float currentHeight = tiles[x, y].Height;
                float regionsCount = _config.regions.Length;

                for (int i = 0; i < regionsCount; i++)
                {
                    if (currentHeight > _config.regions[i].heightLimit)
                        continue;

                    colourMap[y * width + x] = _config.regions[i].colour;
                    break;
                }
            }
        }

        return colourMap;
    }
}
