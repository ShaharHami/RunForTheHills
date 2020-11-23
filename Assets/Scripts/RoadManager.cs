using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoadManager : MonoBehaviour
{
    public int initialTiles;
    public string[] tileObjectNames;
    [Range(0f, 1f)] public float tileSpawnObstaclesProbability;
    [Range(0f, 1f)] public float obstacleSpawnProbability;
    [Range(0f, 1f)] public float pickupSpawnProbability;
    public float specialPickupDivider;
    public int startAfterTile;
    public int sectionLength;
    private float _tileSizeZ;
    internal float _lastTileZ, _firstTileZ;
    private MovePlayer _player;
    private int _currentSection;
    private string _currentTileType;
    private Queue _tileTypesQueue = new Queue();
    private int _tileCount;
    
    private void Awake()
    {
        _player = FindObjectOfType<MovePlayer>();
        AddTileTypesToQueue();
    }

    private void Start()
    {
        _tileSizeZ = Tile.tileZ;
        StartCoroutine(PreWarm());
    }

    private void Update()
    {
        if (_lastTileZ == 0) return;
        var tileType = tileObjectNames[Random.Range(0, tileObjectNames.Length)];
        if (_lastTileZ != 0 && _player.transform.position.z >= (_lastTileZ - _player.minDistanceFront))
        {
            var obj = ObjectPooler.Instance.SpawnFromPool(TileTypeByProgress(_tileCount));
            obj.position = new Vector3(0, 0, _lastTileZ + _tileSizeZ);
            obj.parent = transform;
            PlaceObstacles(obj);
            _lastTileZ += _tileSizeZ;
            _tileCount++;
        }

        foreach (var type in tileObjectNames)
        {
            foreach (var tile in ObjectPooler.Instance._pools[type].Where(tile =>
                tile.gameObject.activeSelf && _player.transform.position.z > tile.position.z + _player.minDistanceBack))
            {
                tile.gameObject.SetActive(false);
            } 
        }
        
    }

    private IEnumerator PreWarm()
    {
        for (int i = 0; i < initialTiles; i++)
        {
            var tileType = TileTypeByProgress(i);
            var t = ObjectPooler.Instance.SpawnFromPool(tileType);

            t.position = new Vector3(0, 0, i * _tileSizeZ);
            t.parent = transform;
            if (i == 0)
            {
                _firstTileZ = t.position.z;
            }

            if (i == initialTiles - 1)
            {
                _lastTileZ = t.position.z;
            }
            PlaceObstacles(t);
            _tileCount++;
            yield return null;
        }
    }

    private void AddTileTypesToQueue()
    {
        foreach (var tileType in tileObjectNames)
        {
            _tileTypesQueue.Enqueue(tileType);
        }
    }
    private string TileTypeByProgress(int i)
    {
        int section = Mathf.RoundToInt(i / sectionLength);
        if (section != _currentSection || string.IsNullOrEmpty(_currentTileType))
        {
            var tileType = _tileTypesQueue.Dequeue().ToString();
            _tileTypesQueue.Enqueue(tileType);
            _currentTileType = tileType;
            _currentSection = section;
            return tileType;
        }
        return _currentTileType;
    }
    private void PlaceObstacles(Transform tile)
    {
        var t = tile.GetComponent<Tile>();
        t.StartSetUp(tileSpawnObstaclesProbability, obstacleSpawnProbability, pickupSpawnProbability, specialPickupDivider, startAfterTile);
    }
}