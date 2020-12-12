using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    internal int score;
    private int _storedHighScore;
    private UIManager _uiManager;
    private MovePlayer _player;
    private DataManager _dataManager;
    private int _previousScoreThreshold;

    private void Awake()
    {
        _uiManager = FindObjectOfType<UIManager>();
        _player = FindObjectOfType<MovePlayer>();
        _dataManager = FindObjectOfType<DataManager>();
    }

    private void Start()
    {
        _storedHighScore = _dataManager.GetHighScore();
    }

    private void OnEnable()
    {
        CollisionHandler.CoinHit += HandleScore;
    }
    private void OnDisable()
    {
        CollisionHandler.CoinHit -= HandleScore;
    }

    private void Update()
    {
        int scoreThreshold = Mathf.RoundToInt(_player.transform.position.z);
        if (scoreThreshold != _previousScoreThreshold)
        {
            HandleScore(1);
            _previousScoreThreshold = scoreThreshold;
        }
    }

    private void HandleScore(int value)
    {
        score += value;
        if (score > _storedHighScore)
        {
            _storedHighScore = score;
        }
        _uiManager.scoreDisplay.text = score.ToString();
    }

    public int FinalScore()
    {
        return score >= _storedHighScore ? score : _storedHighScore;
    }
}
