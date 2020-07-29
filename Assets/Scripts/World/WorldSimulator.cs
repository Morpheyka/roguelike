using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSimulator : MonoBehaviour
{
    [SerializeField] private WorldConfig _config = null;

    private WaitForSecondsRealtime _cycleStep = null;
    private readonly WorldTimer _time = new WorldTimer();

    private void Awake()
    {
        _cycleStep = new WaitForSecondsRealtime(_config.time.timeScale);
        _time.Init(_config.time);
    }

    private void Start()
    {
        StartCoroutine(LifeCycle());
    }

    private IEnumerator LifeCycle()
    {
        while (true)
        {
            _time.Simulate();
            yield return _cycleStep;
        }
    }
}
