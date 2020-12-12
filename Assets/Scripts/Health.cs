using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public static bool dead = false;
    public int lives;
    private MovePlayer _player;
    private UIManager _uiManager;
    private LivesIndicator _livesIndicator;
    private GameManager _gameManager;

    private void Awake()
    {
        _player = GetComponent<MovePlayer>();
        _uiManager = FindObjectOfType<UIManager>();
        _livesIndicator = FindObjectOfType<LivesIndicator>();
        _gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        for (int i = 0; i < lives; i++)
        {
            _livesIndicator.AddLife();
        }
    }

    private void OnEnable()
    {
        CollisionHandler.ObstacleHit += HandleEvent;
    }
    private void OnDisable()
    {
        CollisionHandler.ObstacleHit -= HandleEvent;
    }
    private void HandleEvent(Collider collider)
    {
        lives--;
        _livesIndicator.LoseLife();
        AudioManager.Instance.PlaySfx("Hit");
        if (lives <= 0)
        {
            HandleDeath();
            AudioManager.Instance.PlaySfx("Death");
        }
    }

    private void HandleDeath()
    {
        dead = true;
        _gameManager.HandleEndGame();
        _player.HandleDeath();
    }
}
