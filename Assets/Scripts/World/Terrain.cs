using UnityEngine;

[System.Serializable]
public class Terrain
{
    public TerrainType type;
    public float heightLimit;
    public float heatLimit;
    public Color colour;
}

public enum TerrainType
{
    DeepWater,
    Water,
    Coast,
    Plane,
    Land,
    Hill,
    Mountain
}