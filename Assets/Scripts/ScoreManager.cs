﻿using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    internal int score;
    private UIManager _uiManager;
    private MovePlayer _player;
    private int _previousScoreThreshold;

    private void Awake()
    {
        _uiManager = FindObjectOfType<UIManager>();
        _player = FindObjectOfType<MovePlayer>();
    }

    private void OnEnable()
    {
        CollisionHandler.CoinHit += HandleScore;
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
        _uiManager.scoreDisplay.text = score.ToString();
    }
}
