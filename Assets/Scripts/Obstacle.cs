using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public Transform specialPickUp;
    public Transform effect;
    private MeshRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
    }

    private void OnEnable()
    {
        CollisionHandler.ObstacleHit += HandleObstacleHit;
        effect.gameObject.SetActive(false);
        _renderer.enabled = true;
    }

    private void OnDisable()
    {
        specialPickUp.gameObject.SetActive(false);
    }

    private void HandleObstacleHit(Collider collider)
    {
        if (collider.gameObject == gameObject)
        {
            _renderer.enabled = false;
            effect.gameObject.SetActive(true);
        }
    }
}