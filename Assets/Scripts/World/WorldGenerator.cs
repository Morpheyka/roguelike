using AccidentalNoise;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [Header("Noises")]
    [SerializeField] private Perlin.Data _heightNoise = null;
    [Header("World")]
    [SerializeField] private WorldData _worldData = null;
    [SerializeField] private TerrainConfig _terrains = null;

    public void Generate(int seed)
    {
        _worldData.tiles = new WorldTile[_worldData.size.x, _worldData.size.y];

        Perlin noise = new Perlin();
        float[,] heightMap = noise.Generate(_worldData.size, _heightNoise, seed);

        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        if (_heightNoise.useFalloff)
        {
            Falloff falloff = new Falloff();
            float[,] falloffMap = falloff.Generate(_worldData.size);

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    heightMap[x, y] = Mathf.Clamp01(heightMap[x, y] - falloffMap[x, y]);
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Terrain terrain = new Terrain();

                for (int i = 0; i < _terrains.regions.Length; i++)
                {
                    if (heightMap[x,y] > _terrains.regions[i].heightLimit)
                        continue;

                    terrain.type = _terrains.regions[i].type;
                    terrain.heightLimit = _terrains.regions[i].heightLimit;
                    terrain.colour = _terrains.regions[i].colour;
                    break;
                }

                _worldData.tiles[x, y] = new WorldTile(terrain, heightMap[x, y], 26f, 0f);

                int px = x - 1 < 0 ? -1 : x - 1;
                int nx = x + 1 == width ? -1 : x + 1;
                int py = y - 1 < 0 ? -1 : y - 1;
                int ny = y + 1 == height ? -1 : y + 1;

                WorldTile current = _worldData.tiles[x, y];
                current.leftNeighbour = px == -1 ? null : _worldData.tiles[px, y];
                current.rightNeighbour = nx == -1 ? null : _worldData.tiles[nx, y];
                current.topNeighbour = py == -1 ? null : _worldData.tiles[x, py];
                current.bottomNeighbour = ny == -1 ? null : _worldData.tiles[x, ny];
            }
        }

        FillGroups();

        GetComponent<WorldDisplay>().Draw(_worldData.tiles, _worldData.size);
    }

    private void FillGroups()
    {
        int width = _worldData.size.x;
        int height = _worldData.size.y;

        Stack<WorldTile> tileStack = new Stack<WorldTile>();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                WorldTile current = _worldData.tiles[x, y];

                if (current.alreadyFilled)
                    continue;

                if (current.isCollidable)
                {
                    TerrainGroup group = new TerrainGroup
                    {
                        type = TerrainGroupType.Land
                    };

                    tileStack.Push(current);

                    while (tileStack.Count > 0)
                        FillGroups(tileStack.Pop(), ref group, ref tileStack);

                    if (group.tiles.Count > 0)
                        _worldData.land.Add(group);
                }
                else
                {
                    TerrainGroup group = new TerrainGroup
                    {
                        type = TerrainGroupType.Water
                    };

                    tileStack.Push(current);

                    while (tileStack.Count > 0)
                        FillGroups(tileStack.Pop(), ref group, ref tileStack);

                    if (group.tiles.Count > 0)
                        _worldData.water.Add(group);
                }
            }
        }
    }

    private void FillGroups(WorldTile tile, ref TerrainGroup group, ref Stack<WorldTile> stack)
    {
        if (tile.alreadyFilled)
            return;

        if (group.type == TerrainGroupType.Land && !tile.isCollidable)
            return;

        if (group.type == TerrainGroupType.Water && tile.isCollidable)
            return;

        group.tiles.Add(tile);
        tile.alreadyFilled = true;

        WorldTile t = tile.topNeighbour;
        bool sameGroupAndNotNull = t != null && !t.alreadyFilled 
            && tile.isCollidable == t.isCollidable;

        if (sameGroupAndNotNull)
            stack.Push(t);

        t = tile.bottomNeighbour;
        if (sameGroupAndNotNull)
            stack.Push(t);

        t = tile.leftNeighbour;
        if (sameGroupAndNotNull)
            stack.Push(t);

        t = tile.rightNeighbour;
        if (sameGroupAndNotNull)
            stack.Push(t);
    }
}