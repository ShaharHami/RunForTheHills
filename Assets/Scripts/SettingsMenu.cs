using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider bgmVolume;
    public Slider sfxVolume;

    public void SetBGMVolume(float volume)
    {
        mixer.SetFloat("BGMVolume", Mathf.Log10 (volume) * 20);
    }
    public void SetSFXVolume(float volume)
    {
        mixer.SetFloat("SFXVolume", Mathf.Log10 (volume) * 20);
    }
    
}
