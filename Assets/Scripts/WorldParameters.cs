using System;
using UnityEngine;

[Serializable]
public class WorldParameters : ScriptableObject
{
    public Action OnUpdateCallback { set; private get; }

    // Size Parameters
    [Range(16, 1024), SerializeField] private int width, height;

    public int Width => width;
    public int Height => height;

    // Height Parameters

    [SerializeField] private NoiseParameters heightParameters;
    [Range(0, 10), SerializeField] private float falloffA, falloffB;
    [Range(0, 1), SerializeField] private float falloffMultiplier;
    [SerializeField] private bool normalize;

    // Rain Parameters

    [SerializeField] private NoiseParameters rainParameters;

    // Temperature Parameters

    [Range(0, 10), SerializeField] private float tempA, tempB;
    [Range(0, 1), SerializeField] private float tempHeightRatio;

    // Climate Parameters

    [Range(0, 1), SerializeField] private float seaLevel = .35f, mountainLevel = .65f;

    public float SeaLevel => seaLevel;
    public float MountainLevel => mountainLevel;

    [SerializeField] private Climate seaClimate, mountainClimate;
    [SerializeField] private ClimateZone[] _climateZones;

    public float[,] GetHeightMap(int seed)
    {
        var heightMap = NoiseGenerator.GenerateNoiseMap(width, height, seed, heightParameters, normalize);
        var falloffMap = NoiseGenerator.GetFalloffMap(width, height, falloffA, falloffB);

        for (var y = 0; y < height; y++)
            for (var x = 0; x < width; x++)
                heightMap[x, y] = Mathf.Clamp01(heightMap[x, y] - falloffMap[x, y] * falloffMultiplier);

        return heightMap;
    }

    public Vector2[,] GetSlopeMap(float[,] heightMap)
    {
        var slopeMap = new Vector2[width, height];

        for (var y = 1; y < height - 1; y++)
        {
            for (var x = 1; x < width - 1; x++)
            {
                if (heightMap[x, y] < seaLevel)
                    continue;

                var xSlope = heightMap[x + 1, y] - heightMap[x - 1, y];
                var ySlope = heightMap[x, y + 1] - heightMap[x, y - 1];

                var thing = Mathf.Sqrt(xSlope * xSlope + ySlope * ySlope + 1);
                var normal = new Vector2(-xSlope, -ySlope) / thing;

                slopeMap[x, y] = normal;
            }
        }

        return slopeMap;
    }

    public float[,] GetRainMap(int seed)
    {
        return NoiseGenerator.GenerateNoiseMap(width, height, seed, rainParameters, true);
    }

    public float[,] GetTempMap(float[,] heightMap)
    {
        var tempMap = new float[width, height];

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var gradientTemp = y / (float)height;
                tempMap[x, y] = Mathf.Lerp(NoiseGenerator.Falloff(gradientTemp, tempA, tempB), 0,
                    tempHeightRatio * (heightMap[x, y] - seaLevel));
            }
        }

        return tempMap;
    }

    public Climate[,] GetClimateMap(float[,] heightMap, float[,] tempMap, float[,] rainMap)
    {
        var climateMap = new Climate[width, height];

        var tempTypes = _climateZones.Length;
        var rainTypes = _climateZones[0].Climate.Length;

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var height = heightMap[x, y];

                if (height < seaLevel)
                {
                    climateMap[x, y] = seaClimate;
                }
                else if (height > mountainLevel)
                {
                    climateMap[x, y] = mountainClimate;
                }
                else
                {
                    var temp = Mathf.Clamp(tempMap[x, y], 0f, .99f);
                    var tempIndex = Mathf.FloorToInt(temp * tempTypes);

                    var rain = Mathf.Clamp(rainMap[x, y], 0f, .99f);
                    var rainIndex = Mathf.FloorToInt(rain * rainTypes);

                    climateMap[x, y] = _climateZones[tempIndex].Climate[rainIndex];
                }
            }
        }

        return climateMap;
    }
}
