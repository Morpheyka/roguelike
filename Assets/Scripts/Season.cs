public class Season
{
    public float Heat { get; private set; }

    public Season(SeasonData data, int seed)
    {
        System.Random prnd = new System.Random(seed);

        Heat = prnd.Next(data.minHeat, data.maxHeat);
    }
}