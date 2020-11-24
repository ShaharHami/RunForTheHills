using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPanel : MonoBehaviour
{
    public bool jump;
    private RectTransform _panelTransform;
    private void Awake()
    {
        _panelTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        _panelTransform.sizeDelta = new Vector2(_panelTransform.sizeDelta.x * (jump ? 0.3f : 0.35f), _panelTransform.sizeDelta.y);
    }
}
