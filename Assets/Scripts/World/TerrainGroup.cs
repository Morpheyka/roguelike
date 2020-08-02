using System.Collections.Generic;

public class TerrainGroup
{
    public TerrainGroupType type = default;
    public List<WorldTile> tiles = null;
}

public enum TerrainGroupType
{
    Water,
    Land
}