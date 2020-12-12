using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class FXManager : MonoBehaviour
{
    public static FXManager Instance;
    public float killTimeOut;

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

    public void PlayFX(string fx, Vector3 pos, string value = null)
    {
        StartCoroutine(KillTimer(fx, pos, value));
    }

    private IEnumerator KillTimer(string fx, Vector3 pos, string value)
    {
        var fxGo = ObjectPooler.Instance.SpawnFromPool(fx);
        fxGo.position = pos;
        if (value != null)
        {
            var txt = fxGo.GetComponent<TextMeshPro>();
            if (txt != null)
            {
                txt.text = value;
            }
        }
        yield return new WaitForSeconds(killTimeOut);
        fxGo.gameObject.SetActive(false);
    }

    public void StopFX()
    {
        StopAllCoroutines();
    }
}
