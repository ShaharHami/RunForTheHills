using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int lives;
    private MovePlayer _player;
    private UIManager _uiManager;

    private void Awake()
    {
        _player = GetComponent<MovePlayer>();
        _uiManager = FindObjectOfType<UIManager>();
    }

    private void Start()
    {
        _uiManager.healthDisplay.text = lives.ToString();
    }

    private void OnEnable()
    {
        CollisionHandler.ObstacleHit += HandleEvent;
    }
    private void HandleEvent(Collider collider)
    {
        lives--;
        _uiManager.healthDisplay.text = lives.ToString();
        if (lives <= 0)
        {
            HandleDeath();
        }
    }

    private void HandleDeath()
    {
        _player.speed = 0;
    }
}
