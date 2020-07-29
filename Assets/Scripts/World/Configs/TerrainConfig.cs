using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "terrainConfig", menuName = "World/Terrain")]
public class TerrainConfig : ScriptableObject
{
    public TerrainType[] regions = null;
}
