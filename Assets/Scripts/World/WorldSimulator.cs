using System.Collections;
using UnityEngine;

public class WorldSimulator : MonoBehaviour
{
    [SerializeField] private WorldConfig _config = null;

    private WaitForSecondsRealtime _cycleStep = null;
    private WorldTimer _time = null;

    private void Awake()
    {
        _cycleStep = new WaitForSecondsRealtime(_config.time.timeScale);
        _time = new WorldTimer(_config.time);
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
