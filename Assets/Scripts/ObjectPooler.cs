using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance;
    public List<Pool> pools;
    internal Dictionary<string, List<Transform>> _pools;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }

        _pools = new Dictionary<string, List<Transform>>();
    }

    void Start()
    {
        foreach (var pool in pools)
        {
            _pools[pool.name] = new List<Transform>();
            if (pool.prewarm > 0)
            {
                StartCoroutine(PreWarm(pool.prewarm, pool.name));
            }
        }
    }

    private IEnumerator PreWarm(int prewarm, string objectToPrewarm)
    {
        for (int i = 0; i < prewarm; i++)
        {
            var t = SpawnFromPool(objectToPrewarm);
            t.parent = transform;
            t.gameObject.SetActive(false);
            yield return null;
        }
    }

    public Transform SpawnFromPool(string objectToGet)
    {
        var thisPool = new Pool();
        foreach (var t in pools.Where(t => t.name == objectToGet))
        {
            thisPool = t;
        }
        var p = _pools[objectToGet];
        if (p.Count >= thisPool.prewarm)
        {
            foreach (var obj in p.Where(obj => !obj.gameObject.activeSelf))
            {
                obj.gameObject.SetActive(true);
                return obj;
            }
        }

        if (thisPool.expand)
        {
            if (thisPool.objectToPool == null) return null;
            var obj = Instantiate(thisPool.objectToPool);
            p.Add(obj);
            return obj;
        }

        return null;
    }

    [Serializable]
    public class Pool
    {
        public string name;
        public Transform objectToPool;
        public int prewarm;
        public bool expand;
    }
}