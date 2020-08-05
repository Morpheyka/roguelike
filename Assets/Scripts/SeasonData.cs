using UnityEngine;

[System.Serializable]
public class SeasonData
{
    public string name = string.Empty;
    [Range(-50f, 49f)]public int minHeat = 0;
    [Range(-49f, 50f)]public int maxHeat = 0;
    public Month[] months = null;
}