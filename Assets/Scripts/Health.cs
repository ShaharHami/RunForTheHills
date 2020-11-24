using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public static bool dead = false;
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
        AudioManager.Instance.PlaySfx("Hit");
        if (lives <= 0)
        {
            HandleDeath();
            AudioManager.Instance.PlaySfx("Death");
        }
    }

    private void HandleDeath()
    {
        _player.speed = 0;
        dead = true;
    }
}
