using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class ObjectPooler : MonoBehaviour
{
    public Transform objectToPool;
    public int prewarm;
    public bool expand;
    [HideInInspector] public float zSize, xSize;
    [HideInInspector] public float _lastTileZ, _firstTileZ;
    public List<Transform> Pool;
    private MovePlayer _player;
    
    private void Awake()
    {
        zSize = objectToPool.GetComponent<Tile>().gameTile.transform.localScale.z;
        xSize = objectToPool.GetComponent<Tile>().gameTile.transform.localScale.x;
        Pool = new List<Transform>();
        _player = FindObjectOfType<MovePlayer>();
    }

    void Start()
    {
        if (prewarm > 0)
        {
            StartCoroutine(PreWarm());
        }
    }

    private IEnumerator PreWarm()
    {
        for (int i = 0; i < prewarm; i++)
        {
            var t = SpawnFromPool();
            t.position = new Vector3(0, 0, i * zSize);
            t.parent = transform;
            if (i == 0)
            {
                _firstTileZ = t.position.z;
            }
            if (i == prewarm-1)
            {
                _lastTileZ = t.position.z;
            }
            yield return null;
        }
    }

    public Transform SpawnFromPool()
    {
        if (Pool.Count > 0)
        {
            foreach (var obj in Pool.Where(obj => !obj.gameObject.activeSelf))
            {
                obj.gameObject.SetActive(true);
                return obj;
            }
        }
        if (expand)
        {
            var obj = Instantiate(objectToPool).transform;
            Pool.Add(obj);
            return obj;
        }

        return null;
    }
}
