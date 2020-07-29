using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [Header("Noises")]
    [SerializeField] private Perlin.Data _heightNoise = null;
    [Header("World")]
    [SerializeField] private WorldData _worldData = null;

    public void Generate(int seed)
    {
        GenerateMaps(seed);
        GetComponent<WorldDisplay>().Draw(_worldData);
    }

    private void GenerateMaps(int seed)
    {
        Perlin noise = new Perlin();
        _worldData.HeightMap = noise.Generate(_worldData.Size, _heightNoise, seed);

        Falloff falloff = new Falloff();
        _worldData.FalloffMap = falloff.Generate(_worldData.Size);
    }
}