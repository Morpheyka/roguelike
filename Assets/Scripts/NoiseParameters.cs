using System;
using UnityEngine;

[Serializable]
public struct NoiseParameters
{
    [Range(1, 8)]
    public int octaves;

    [Range(0, 1)]
    public float persistance;

    [Range(1, 5)]
    public float lacunarity;

    [Range(10, 200)]
    public int scale;
}
