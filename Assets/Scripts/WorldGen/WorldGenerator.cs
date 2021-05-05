using System;
using UnityEngine;
using UnityEngine.Events;

public class WorldGenerator : MonoBehaviour
{

    [SerializeField] private bool generateWorldOnUpdate;
    [SerializeField] private int seed;
    [SerializeField] private WorldParameters worldParameters;

    public UnityEvent onWorldGenerated;

    private void Start()
    {
        GenerateWorld();
    }

    private void GenerateWorld()
    {
        World.GenerateWorld(seed, worldParameters, onWorldGenerated.Invoke, OnParameterUpdated);
    }

    private void OnParameterUpdated()
    {
        if (generateWorldOnUpdate)
            GenerateWorld();
    }
}