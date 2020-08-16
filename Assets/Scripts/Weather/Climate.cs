using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climate
{
    private readonly WorldTile[,] _world = null;
    private readonly Vector2Int _startPosition = default;
    private readonly int _climateRectangleWidth = 0;
    private readonly int _climateRectangleLenght = 0;

    private SeasonData _currentSeason = null;
    private uint _seasonLenght = 0;
    private int _seasonDaysGone = 0;
    private int _simulateStepsCount = 0;

    public Climate(WorldTile[,] world, SeasonData season)
    {
        _world = world;
        _currentSeason = season;
        _startPosition = ChooseStartPosition();
        _climateRectangleWidth = world.GetLength(0);

        foreach (Month month in _currentSeason.months)
            _seasonLenght += month.daysCount;

        _climateRectangleLenght = world.GetLength(0) * world.GetLength(1) +
            (world.GetLength(0) * world.GetLength(1) / 100 * 20);
        _simulateStepsCount = Mathf.CeilToInt(_climateRectangleLenght / _seasonLenght);
    }

    private Vector2Int ChooseStartPosition()
    {
        Vector2Int[] requiredPositions = new Vector2Int[6];
            
        int width = _world.GetLength(0);
        int height = _world.GetLength(1);

        requiredPositions[0] = new Vector2Int(0, 0); // left bottom
        requiredPositions[1] = new Vector2Int(0, height - 1); //left top
        requiredPositions[2] = new Vector2Int(width - 1, 0); //right bottom
        requiredPositions[3] = new Vector2Int(width - 1, height - 1); //ritght top
        requiredPositions[4] = new Vector2Int(0, (height - 1) / 2); //middle right
        requiredPositions[5] = new Vector2Int(width - 1, (height - 1) / 2); //middle left

        //return requiredPositions[Random.Range(0, requiredPositions.Length)];
        return requiredPositions[0]; //DEBUG
    }

    public void Simulate()
    {
        int width = _world.GetLength(0);
        int height = (_startPosition.y + _seasonDaysGone + 1) * 
            Mathf.CeilToInt(_world.GetLength(1) / _seasonLenght);

        Debug.LogWarning(_seasonDaysGone);
        Debug.LogWarning(height);

        for (int y = _startPosition.y; y < height; y++)
        {
            for (int x = _startPosition.x; x < width; x++)
            {
                _world[x, y].IncHeat(5);
                Debug.LogWarning(_world[x, y].Heat);
            }
        }

        _seasonDaysGone++;
    }

    public void ChangeSeason(SeasonData targetSeason)
    {
        _currentSeason = targetSeason;
        _seasonDaysGone = 0;

        foreach (Month month in _currentSeason.months)
            _seasonLenght += month.daysCount;

        _simulateStepsCount = Mathf.CeilToInt(_climateRectangleLenght / _seasonLenght);
    }
}