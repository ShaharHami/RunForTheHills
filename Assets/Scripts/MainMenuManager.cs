using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MainMenuManager : MonoBehaviour
{
    public TextMeshProUGUI highScoreText;
    private DataManager _dataManager;

    private void Awake()
    {
        _dataManager = FindObjectOfType<DataManager>();
    }

    private void Start()
    {
        AudioManager.Instance.hit = true;
        highScoreText.text = _dataManager.GetHighScore().ToString();
    }
    
    public void ResetScore()
    {
        PlayButtonSound();
        _dataManager.ResetScore();
        highScoreText.text = _dataManager.GetHighScore().ToString();
    }
    public void PlayGame()
    {
        PlayButtonSound();
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
    }
    public void QuitGame()
    {
        PlayButtonSound();
        Application.Quit();
    }

    public void GoToURL(string url)
    {
        Application.OpenURL(url);
    }
    private void PlayButtonSound()
    {
        AudioManager.Instance.PlaySfx("ButtonClick");
    }
}
