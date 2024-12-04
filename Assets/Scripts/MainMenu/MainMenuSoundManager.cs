using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSoundManager : MonoBehaviour
{
    public static MainMenuSoundManager Instance { get; private set; }
    [SerializeField] private AudioSource BgmSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip bgmClip;
    [SerializeField] private AudioClip resumeClip;
    [SerializeField] private AudioClip exitClip;
    [SerializeField] private AudioClip clickClip;

    public bool isMusicOn = true;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        sfxSource.loop = false;
        PlayBGM();
    }

    public void PlayBGM()
    {
        if (bgmClip != null)
        {
            BgmSource.clip = bgmClip;
            BgmSource.loop = true;
            BgmSource.Play();
        }
    }

    public void ToggleMusic()
    {
        isMusicOn = !isMusicOn;
        if (isMusicOn) BgmSource.Play();
        else BgmSource.Pause();
    }


    public void PlayResumeClipSound()
    {
        sfxSource.PlayOneShot(resumeClip);

    }

    public void PlayExitClipSound()
    {
        sfxSource.PlayOneShot(exitClip);

    }

    public void PlayClickClipSound()
    {
        sfxSource.PlayOneShot(clickClip);
    }
    public void OnVolumeChanged(float volume)
    {
        BgmSource.volume = volume;
        sfxSource.volume = volume;
    }
}
