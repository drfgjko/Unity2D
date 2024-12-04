using UnityEngine;

public class GameBgmManager : MonoBehaviour
{
    public static GameBgmManager Instance { get; private set; }

    [Header("Source")]
    [SerializeField] private AudioSource BgmSource;
    [SerializeField] private AudioSource SfxSource;

    [Header("Clip")]
    [SerializeField] private AudioClip bgmClip;
    [SerializeField] private AudioClip loseBgmClip;
    [SerializeField] private AudioClip successBgmClip;
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip getScoreClip;
    [SerializeField] private AudioClip finishQteClip;
    [SerializeField] private AudioClip getBonusClip;
    [SerializeField] private AudioClip musicToggleClip;
    [SerializeField] private AudioClip pauseClip;
    [SerializeField] private AudioClip resumeClip;
    [SerializeField] private AudioClip exitClip;
    public bool isMusicOn = true;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SfxSource.loop = false;
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
    public void PlayLoseBGM()
    {
        if (loseBgmClip != null)
        {
            BgmSource.clip = loseBgmClip;
            BgmSource.loop = true;
            BgmSource.Play();
        }
    }
    public void PlaySuccessBgmClip()
    {
        if (loseBgmClip != null)
        {
            BgmSource.clip = successBgmClip;
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

    public void PlayJumpSound()
    {
        SfxSource.PlayOneShot(jumpClip);
    }
    public void PlayShootSound()
    {
        SfxSource.PlayOneShot(shootClip);
    }
    public void PlayGetScoreSound()
    {
        SfxSource.PlayOneShot(getScoreClip);
    }
    public void PlayGetBonusSound()
    {
        SfxSource.PlayOneShot(getBonusClip);
    }
    public void PlayFinishoQteSound()
    {
        SfxSource.PlayOneShot(finishQteClip);

    }

    public void PlayMusicToggleClipSound()
    {
        SfxSource.PlayOneShot(musicToggleClip);

    }
    public void PlayPauseClipSound()
    {
        SfxSource.PlayOneShot(pauseClip);

    }
    public void PlayResumeClipSound()
    {
        SfxSource.PlayOneShot(resumeClip);
    }
    public void PlayExitClipSound()
    {
        SfxSource.PlayOneShot(exitClip);

    }
    public void OnVolumeChanged(float volume)
    {
        BgmSource.volume = volume;
        SfxSource.volume = volume;
    }

}
