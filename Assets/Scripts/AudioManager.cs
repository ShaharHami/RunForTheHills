using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public List<AudioSample> samples;
    public List<AudioClip> footSteps;
    public float delayBetweenSteps;
    public AudioSource bgmSource;
    public AudioSource sfxSource;
    public AudioSource stepsSource;
    [HideInInspector] public bool hit;
    [HideInInspector] public Coroutine stepsCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        bgmSource.Play();
    }

    public void StartFootsteps()
    {
        if (stepsCoroutine != null)
        {
            StopCoroutine(stepsCoroutine);
        }

        hit = false;
        stepsCoroutine = StartCoroutine(PlayFootSteps());
    }

    public void PlaySfx(string sampleName, bool bgm = false)
    {
        foreach (var sample in samples)
        {
            if (sample.name == sampleName)
            {
                if (bgm)
                {
                    bgmSource.clip = sample.clip;
                    bgmSource.Play();
                }

                if (sample.volume > 0)
                {
                    sfxSource.volume = sample.volume;
                }
                else
                {
                    sfxSource.volume = 1;
                }

                sfxSource.pitch = Random.Range(sample.pitchX, sample.pitchY);
                sfxSource.PlayOneShot(sample.clip);
            }
        }
    }

    private IEnumerator PlayFootSteps()
    {
        while (true)
        {
            if (!Health.dead && !hit)
            {
                stepsSource.PlayOneShot(footSteps[Random.Range(0, footSteps.Count)]);
            }
            yield return new WaitForSeconds(delayBetweenSteps);
        }
    }

    public void ToggleFootSteps(bool on)
    {
        hit = !on;
    }

    [Serializable]
    public class AudioSample
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1;
        [Range(-3f, 3f)] public float pitchX = 1;
        [Range(-3f, 3f)] public float pitchY = 1;
        public bool repeat;
    }
}