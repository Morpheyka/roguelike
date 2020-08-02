using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorldData
{
    public Vector2Int size = default;
    public WorldTile[,] tiles = null;

    public List<TerrainGroup> water = new List<TerrainGroup>();
    public List<TerrainGroup> land = new List<TerrainGroup>();
}