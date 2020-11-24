using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float levelIncreaseInterval;
    public float speedIncrement;
    public float tileObstacleSpawnIncrement;
    public float obstacleSpawnIncrement;
    public float pickupSpawnIncrement;
    public float maxPlayerSpeed;
    private RoadManager _roadManager;
    private MovePlayer _player;
    private int _previousIncreasePassed;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        _roadManager = FindObjectOfType<RoadManager>();
        _player = FindObjectOfType<MovePlayer>();
    }

    private void Update()
    {
        int increasePassed = Mathf.RoundToInt(_player.transform.position.z / levelIncreaseInterval);
        
        if (increasePassed != _previousIncreasePassed)
        {
            if (_roadManager.tileSpawnObstaclesProbability > 0 && _roadManager.tileSpawnObstaclesProbability < 1)
            {
                _roadManager.tileSpawnObstaclesProbability += tileObstacleSpawnIncrement;
            }
            if (_roadManager.obstacleSpawnProbability > 0 && _roadManager.obstacleSpawnProbability < 1)
            {
                _roadManager.obstacleSpawnProbability += obstacleSpawnIncrement;
            }
            if (_roadManager.pickupSpawnProbability > 0 && _roadManager.pickupSpawnProbability < 1)
            {
                _roadManager.pickupSpawnProbability += pickupSpawnIncrement;
            }
            if (_player.speed > 0 && _player.speed < maxPlayerSpeed)
            {
                _player.speed += speedIncrement;
            }

            _previousIncreasePassed = increasePassed;
        }
    }
}
