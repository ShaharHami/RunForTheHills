using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int startUpDelay;
    public float levelIncreaseInterval;
    public float speedIncrement;
    public float tileObstacleSpawnIncrement;
    public float obstacleSpawnIncrement;
    public float pickupSpawnIncrement;
    public float maxPlayerSpeed;
    public GameObject daveEasterEgg;
    public float easterEggDuration;
    public Popup popup;
    private DataManager _dataManager;
    private RoadManager _roadManager;
    private MovePlayer _player;
    private ScoreManager _scoreManager;
    private UIManager _uiManager;
    private FXManager _fxManager;
    private int _previousIncreasePassed;
    internal bool gameHasStarted;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        _dataManager = FindObjectOfType<DataManager>();
        _roadManager = FindObjectOfType<RoadManager>();
        _player = FindObjectOfType<MovePlayer>();
        _scoreManager = FindObjectOfType<ScoreManager>();
        _uiManager = FindObjectOfType<UIManager>();
        _fxManager = FindObjectOfType<FXManager>();
    }

    private void Start()
    {
        gameHasStarted = false;
        AudioManager.Instance.hit = true;
        _player.ToggleMovement(false);
        StartCoroutine(StartUpSequence());
    }

    private IEnumerator StartUpSequence()
    {
        for (int i = startUpDelay; i > -1; i--)
        {
            _uiManager.countDownDisplay.text = i.ToString();
            AudioManager.Instance.PlaySfx(i == 0 ? "LastCountDownBeep" : "CountDownBeep");
            yield return new WaitForSeconds(1);    
        }
        AudioManager.Instance.hit = false;
        _uiManager.countDownDisplay.gameObject.SetActive(false);
        _player.ToggleMovement(true);
        _player.StartMoving();
        gameHasStarted = true;
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

        _uiManager.speedDisplay.text = _player.speed.ToString("F1");
    }

    public void HandleEndGame()
    {
        _dataManager.SaveHighScore(_scoreManager.FinalScore());
        StartCoroutine(ShowEndGamePopup());
    }

    private IEnumerator ShowEndGamePopup()
    {
        yield return new WaitForSeconds(2);
        popup.OpenGameOverScreen();
    }
    public void PauseGame()
    {
        AudioManager.Instance.hit = true;
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        if (gameHasStarted)
        {
            AudioManager.Instance.hit = false;
        }
        Time.timeScale = 1;
    }

    public void BackToMainMenu()
    {
        _fxManager.StopFX();
        PlayButtonSound();
        SceneManager.LoadScene(0);
        ResumeGame();
    }

    public void RestartGame()
    {
        PlayButtonSound();
        ResumeGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    private void PlayButtonSound()
    {
        AudioManager.Instance.PlaySfx("ButtonClick");
    }
    
    // easter egg
    public void ShowEasterEgg()
    {
        AudioManager.Instance.PlaySfx("DaveButtonClick");
        StartCoroutine(ShowAndHideEasterEgg());
    }

    private IEnumerator ShowAndHideEasterEgg()
    {
        daveEasterEgg.SetActive(true);
        yield return new WaitForSeconds(easterEggDuration);
        daveEasterEgg.SetActive(false);
    }
}
