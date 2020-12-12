using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Obstacle : MonoBehaviour
{
    public Transform specialPickUp;
    public Transform effect;
    public MeshRenderer obstacleRenderer;
    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        CollisionHandler.ObstacleHit += HandleObstacleHit;
        effect.gameObject.SetActive(false);
        obstacleRenderer.enabled = true;
        _collider.enabled = true;
    }

    private void OnDisable()
    {
        CollisionHandler.ObstacleHit -= HandleObstacleHit;
        specialPickUp.gameObject.SetActive(false);
    }

    private void HandleObstacleHit(Collider collider)
    {
        if (collider.gameObject == gameObject)
        {
            _collider.enabled = false;
            obstacleRenderer.enabled = false;
            effect.gameObject.SetActive(true);
        }
    }
}