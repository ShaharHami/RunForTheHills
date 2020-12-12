using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private const string SaveKey = "HighScore";
    public void SaveHighScore(int highscore)
    {
        PlayerPrefs.SetInt(SaveKey, highscore);
        PlayerPrefs.Save();
    }

    public int GetHighScore()
    {
        if (PlayerPrefs.HasKey(SaveKey))
        {
            return PlayerPrefs.GetInt(SaveKey);
        }
        return 0;
    }

    public void ResetScore()
    {
        if (PlayerPrefs.HasKey(SaveKey))
        {
            PlayerPrefs.DeleteKey(SaveKey);
        }
    }
}
