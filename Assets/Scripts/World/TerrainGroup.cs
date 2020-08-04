using System.Collections.Generic;

public class TerrainGroup
{
    public TerrainGroupType type = default;
    public List<WorldTile> tiles = new List<WorldTile>();
}

public enum TerrainGroupType
{
    Water,
    Land
}