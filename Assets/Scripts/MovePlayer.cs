using System.Linq;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public float speed;
    public float minDistanceFront, minDistanceBack;
    private ObjectPooler _pooler;
    private float _tileSizeZ, _tileSizeX;

    private void Awake()
    {
        _pooler = FindObjectOfType<ObjectPooler>();
    }

    private void Start()
    {
        _tileSizeZ = _pooler.zSize;
        _tileSizeX = _pooler.xSize;
        // lastPlacedTileZ = _tileSizeZ * (_pooler.prewarm-1);
    }

    void Update()
    {
        if (_pooler._lastTileZ == 0) return;
        transform.position += new Vector3(0, 0, speed * Time.deltaTime);
        
        if (_pooler._lastTileZ != 0 && transform.position.z >= (_pooler._lastTileZ - minDistanceFront))
        {
            var obj = _pooler.SpawnFromPool();
            obj.position = new Vector3(0, 0, _pooler._lastTileZ + _tileSizeZ);
            obj.parent = _pooler.transform;
            _pooler._lastTileZ += _tileSizeZ;
        }

        foreach (var tile in _pooler.Pool.Where(tile => tile.gameObject.activeSelf && transform.position.z > tile.position.z + minDistanceBack))
        {
            tile.gameObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (transform.position.x > -1)
            {
                transform.position -= new Vector3(1, 0, 0);
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (transform.position.x < 1)
            {
                transform.position += new Vector3(1, 0, 0);
            }
        }
    }
}
