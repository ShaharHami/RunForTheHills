using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FXManager : MonoBehaviour
{
    public static FXManager Instance;
    [FormerlySerializedAs("_killTimeOut")] public float killTimeOut;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    public void PlayFX(string fx, Vector3 pos)
    {
        StartCoroutine(KillTimer(fx, pos));
    }

    private IEnumerator KillTimer(string fx, Vector3 pos)
    {
        var fxGo = ObjectPooler.Instance.SpawnFromPool(fx);
        fxGo.position = pos;
        yield return new WaitForSeconds(killTimeOut);
        fxGo.gameObject.SetActive(false);
    }

}
