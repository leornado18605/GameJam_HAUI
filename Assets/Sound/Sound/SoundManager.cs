using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Music")]
    public AudioClip musicClip;

    [Header("Zombie")]
    public AudioClip breathClip;
    public AudioClip runClip;
    public AudioClip hitClip;

    [Header("EndGame")]
    public AudioClip endGameClip;

    [Header("Button")]
    public AudioClip btnClip;

    [Header("Weapon")]
    public AudioClip playClip;


    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        playMusic();
    }

    public void playMusic()
    {
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlaySFX(AudioClip audioClip)
    {
        sfxSource.PlayOneShot(audioClip);
    }
}
