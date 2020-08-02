using UnityEngine;

[System.Serializable]
public class Terrain
{
    public TerrainType type;
    public float heightLimit;
    public Color colour;
}

public enum TerrainType
{
    Ocean,
    Lake,
    Beatch,
    Land,
    Mountain
}