using UnityEngine;

[CreateAssetMenu(fileName = "worldConfig", menuName = "World/Simulation")]
public class WorldConfig : ScriptableObject
{
    public TimeData time = null;

    [System.Serializable]
    public class TimeData
    {
        public float timeScale = 0.33f;
        public int minutesPerSimulate = 3;
        public Month[] months = null;
    }
}

[System.Serializable]
public class Month
{
    public string name = string.Empty;
    public int daysCount = 30;
}