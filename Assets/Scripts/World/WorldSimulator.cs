using System.Collections;
using UnityEngine;

public class WorldSimulator : MonoBehaviour
{
    [SerializeField] private WorldConfig _config = null;
    [SerializeField] private WorldGenerator _generator = null;

    private WaitForSecondsRealtime _cycleStep = null;
    private WorldTimer _time = null;
    private Climate _weather = null;

    private void Awake()
    {
        _cycleStep = new WaitForSecondsRealtime(_config.time.timeScale);
        _time = new WorldTimer(_config.time);
        _weather = new Climate(_generator.Generate(0).tiles, _time.CurrentSeason);

        _time.OnDayChange += _weather.Simulate;
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
