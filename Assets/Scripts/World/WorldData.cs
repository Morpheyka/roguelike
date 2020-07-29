using UnityEngine;

[System.Serializable]
public class WorldData
{
    public Vector2Int Size = default;
    public float[,] HeightMap = null;
    public float[,] ClimateMap = null;
    public float[,] HumidityMap = null;
    public float[,] FalloffMap = null;
}