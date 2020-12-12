using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ResetSayolloScreen : MonoBehaviour
{
    public Material replacementMaterial;
    private Renderer _renderer;
    private GameObject _sayolloMainObject;
    private Shader _shader;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _shader = Shader.Find("UI/Default");
    }

    private void OnEnable()
    {
        StartCoroutine(ReplaceMaterial());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator ReplaceMaterial()
    {
        while (true)
        {
            if (_renderer.material.shader != _shader)
            {
                _renderer.material.shader = _shader;
            }
            yield return new WaitForSeconds(2f);
        }
    }
}
