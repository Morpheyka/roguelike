using UnityEngine;

[CreateAssetMenu(fileName = "worldConfig", menuName = "World/Simulation")]
public class WorldConfig : ScriptableObject
{
    public TimeData time = null;
}