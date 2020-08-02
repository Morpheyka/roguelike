[System.Serializable]
public class WorldTile
{
    public float Height { get; private set; }
    public float Temperature { get; private set;}
    public float Humidity { get; private set; }

    public Terrain terrain = default;
    public WorldTile leftNeighbour = null;
    public WorldTile rightNeighbour = null;
    public WorldTile topNeighbour = null;
    public WorldTile bottomNeighbour = null;

    public bool isCollidable = false;
    public bool alreadyFilled = false;
    private int _bitMask = 0;

    public WorldTile(Terrain terrain, float height, float temperature, float humidity)
    {
        this.terrain = terrain;

        Height = height;
        Temperature = temperature;
        Humidity = humidity;
    }

    public void UpdateBitMask()
    {
        int count = 0;

        if (topNeighbour.terrain == terrain)
            count++;

        if (rightNeighbour.terrain == terrain)
            count += 2;

        if (bottomNeighbour.terrain == terrain)
            count += 4;

        if (topNeighbour.terrain == terrain)
            count += 8;

        _bitMask = count;
    }
}