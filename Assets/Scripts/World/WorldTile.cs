[System.Serializable]
public class WorldTile
{
    public float Height { get; private set; }
    public float Heat { get; private set;}
    public float Humidity { get; private set; }
    public int Mask => _bitMask;

    public Terrain terrain = default;
    public WorldTile leftNeighbour = null;
    public WorldTile rightNeighbour = null;
    public WorldTile topNeighbour = null;
    public WorldTile bottomNeighbour = null;

    public bool isCollidable = false;
    public bool alreadyFilled = false;
    private int _bitMask = 0;

    public WorldTile(Terrain terrain, float height, float heat, float humidity)
    {
        this.terrain = terrain;

        Height = height;
        Heat = heat;
        Humidity = humidity;
    }

    public void UpdateBitMask()
    {
        int count = 0;

        if (leftNeighbour != null && leftNeighbour.terrain == terrain)
            count++;

        if (rightNeighbour != null && rightNeighbour.terrain == terrain)
            count += 2;

        if (bottomNeighbour != null && bottomNeighbour.terrain == terrain)
            count += 4;

        if (topNeighbour != null && topNeighbour.terrain == terrain)
            count += 8;

        _bitMask = count;
    }

    public void IncHeat(int value)
    {
        Heat += value;
    }
}