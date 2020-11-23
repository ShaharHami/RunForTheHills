using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiftTiling : MonoBehaviour
{
    public Vector2 shiftTilingBy;
    private Renderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        _renderer.material.mainTextureOffset += shiftTilingBy * Time.deltaTime;
    }
}
