using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    public GameObject settingsMenu;
    public GameObject gameOverScreen;
    public GameObject closeButton;
    public Button openMenuButton;

    private void OnEnable()
    {
        openMenuButton.interactable = true;
    }

    public void OpenSettingsMenu()
    {
        PlayButtonSound();
        gameObject.SetActive(true);
        closeButton.SetActive(true);
        gameOverScreen.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void OpenGameOverScreen()
    {
        gameObject.SetActive(true);
        openMenuButton.interactable = false;
        closeButton.SetActive(false);
        settingsMenu.SetActive(false);
        gameOverScreen.SetActive(true);
    }

    public void ClosePopup()
    {
        PlayButtonSound();
        gameObject.SetActive(false);
    }
    private void PlayButtonSound()
    {
        AudioManager.Instance.PlaySfx("ButtonClick");
    }
}
