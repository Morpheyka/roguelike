using UnityEngine;


[RequireComponent(typeof(Collider))]
public class WorldClouds : MonoBehaviour
{
    [Header("Cloud")]
    [SerializeField] private Vector3 _minSize = default;
    [SerializeField] private Vector3 _maxSize = default;
    [SerializeField] private float _height = 10f;

    private Collider _bounds = null;

    private void Awake()
    {
        _bounds = GetComponent<Collider>();
    }

    private void Start()
    {
        Spawn();
    }

    private void Spawn()
    {
        var width = _bounds.bounds.size.x;
        var height = _bounds.bounds.size.z;
        var start = new Vector3(_bounds.bounds.min.x, 0f, _bounds.bounds.max.z);
        var material = new Material(Shader.Find("Unlit/Color"));

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var cloud = GameObject.CreatePrimitive(PrimitiveType.Cube);

                var posX = start.x + cloud.transform.localScale.x * x;
                var posY = _height;
                var posZ = start.z - cloud.transform.localScale.z * y;

                cloud.transform.position = new Vector3(posX, posY, posZ);
                cloud.GetComponent<Renderer>().sharedMaterial = material;
            }
        }
    }
}
