using UnityEngine;

public class WorldDisplay : MonoBehaviour
{   
    public enum DrawMode { Perlin, ColourPerlin, Main, Falloff, Humidity }

    [Header("Prefabs")]
    [SerializeField] private GameObject _texturePrefab = null;
    [SerializeField] private GameObject _meshPrefab = null;
    [Header("Properties")]
    [SerializeField] private DrawMode _mode = DrawMode.Perlin;
    [SerializeField] private TerrainConfig _config = null;
    [SerializeField] private float _heightMultiplier = 1f;
    [SerializeField] private AnimationCurve _heightCurve = null;
    [SerializeField] private bool _useFallow = false;

    private GameObject _targetDisplay = null;
    private Renderer _render = null;
    private MeshRenderer _meshRender = null;
    private MeshFilter _meshFilter = null;

    private void Prepare()
    {
        if (_targetDisplay != null)
            Destroy(_targetDisplay.gameObject);

        if (_mode == DrawMode.Main)
        {
            _targetDisplay = Instantiate(_meshPrefab, transform);
            _meshRender = _targetDisplay.GetComponent<MeshRenderer>();
            _meshFilter = _targetDisplay.GetComponent<MeshFilter>();
        }
        else
        {
            _targetDisplay = Instantiate(_texturePrefab, transform);
            _render = _targetDisplay.GetComponent<Renderer>();
        }
    }

    public void Draw(WorldData data)
    {
        Prepare();

        switch (_mode)
        {
            case DrawMode.Perlin:
                DrawNoiseTexture(data.HeightMap, data.Size, Color.black, Color.white);
                break;
            case DrawMode.ColourPerlin:
                DrawColourTexture(data);
                break;
            case DrawMode.Main:
                DrawMesh(data);
                break;
            case DrawMode.Falloff:
                DrawNoiseTexture(data.FalloffMap, data.Size, Color.black, Color.white);
                break;
            case DrawMode.Humidity:
                DrawNoiseTexture(data.HeightMap, data.Size, Color.yellow, Color.blue);
                break;
        }
    }

    private void DrawNoiseTexture(float[,] map, Vector2Int size, Color lower, Color height)
    {
        _render.sharedMaterial.mainTexture = TextureGenerator.CreateTerrainTexture(map, lower, height);
        _render.transform.localScale = new Vector3(size.x, 1f, size.y);
    }

    private void DrawColourTexture(WorldData data)
    {
        Color[] colourMap = GenerateColourMap(data);

        _render.sharedMaterial.mainTexture = TextureGenerator.CreateColourTerrainTexture(data.Size, colourMap);
        _render.transform.localScale = new Vector3(data.Size.x, 1f, data.Size.y);
    }

    private Color[] GenerateColourMap(WorldData data)
    {
        int width = data.Size.x;
        int height = data.Size.y;

        Color[] colourMap = new Color[width * height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (_useFallow)
                    data.HeightMap[x, y] = Mathf.Clamp01(data.HeightMap[x, y] - data.FalloffMap[x, y]);

                float currentHeight = data.HeightMap[x, y];
                float regionsCount = _config.regions.Length;

                for (int i = 0; i < regionsCount; i++)
                {
                    if (currentHeight > _config.regions[i].height)
                        continue;

                    colourMap[y * width + x] = _config.regions[i].colour;
                    break;
                }
            }
        }

        return colourMap;
    }

    private void DrawMesh(WorldData data)
    {
        Color[] colourMap = GenerateColourMap(data);
        _meshRender.sharedMaterial.mainTexture = TextureGenerator.CreateColourTerrainTexture(data.Size, colourMap);

        MeshGenerator.MeshData meshData = MeshGenerator.GenerateTerrainMesh(data.HeightMap, _heightMultiplier, _heightCurve);
        _meshFilter.sharedMesh = MeshGenerator.GenerateMesh(meshData);
    }
}
