using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class TutorialUI : MonoBehaviour
{
    public static TutorialUI instance;
    [Header("Main Details")]
    [SerializeField] private TextMeshProUGUI amoText;
    [SerializeField] private GameObject exitBtn;

    [Header("PausePanel details")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject resumeButton;
    [SerializeField] private GameObject quitButton;
    private bool isPaused = false;

    [Header("Qte details")]
    [SerializeField] private int reloadSteps = 5;
    [SerializeField] private BoxCollider2D qteBtnWindows;
    [SerializeField] private GunController gunController;
    [SerializeField] private QteButton[] qteButtons;
    [SerializeField] private int currentSteps = 0;

    [Header("BtnUI details")]
    [SerializeField] private GameObject musicButton;
    [SerializeField] private Image musicButtonIcon;
    [SerializeField] private Sprite[] musicBtnSprites;
    [SerializeField] private Slider volumeSlider;

    [Header("Cursor details")]
    [SerializeField] private Texture2D customCursorTexture;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        qteButtons = GetComponentsInChildren<QteButton>(true);
        volumeSlider.value = 100;
        InitPanel();
        InitMusicBtn();
        InitCursor();
        InitButtonsListener();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            OpenReloadUI();
            Time.timeScale = 0.7f;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }
    private void InitButtonsListener()
    {
        resumeButton.GetComponent<Button>().onClick.AddListener(ResumeGame);
        quitButton.GetComponent<Button>().onClick.AddListener(QuitGame);
        musicButton.GetComponent<Button>().onClick.AddListener(UpdateMusicButton);
        exitBtn.GetComponent<Button>().onClick.AddListener(QuitGame);
    }
    private void InitPanel()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }
    private void InitCursor()
    {
        Cursor.SetCursor(customCursorTexture, Vector2.zero, CursorMode.Auto);
    }
    private void ResumeGame()
    {
        GameBgmManager.Instance.PlayResumeClipSound();
        pausePanel.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;

    }

    private void PauseGame()
    {
        GameBgmManager.Instance.PlayPauseClipSound();
        pausePanel.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
    }
    private void QuitGame()
    {
        GameData.InitOldGameObject();
        GameBgmManager.Instance.PlayExitClipSound();
        SceneManager.LoadScene("Menu");
    }
    public void OpenReloadUI()
    {
        List<QteButton> buttonList = new(qteButtons);
        Shuffle(buttonList);
        for (int i = 0; i < reloadSteps; i++)
        {
            QteButton button = buttonList[i];
            button.gameObject.SetActive(true);

            float randomX = Random.Range(qteBtnWindows.bounds.min.x, qteBtnWindows.bounds.max.x);
            float randomY = Random.Range(qteBtnWindows.bounds.min.y, qteBtnWindows.bounds.max.y);
            button.transform.position = new Vector2(randomX, randomY);
        }

        for (int i = reloadSteps; i < buttonList.Count; i++)
        {
            buttonList[i].gameObject.SetActive(false);
        }
        currentSteps = reloadSteps;
    }

    private void Shuffle(List<QteButton> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            QteButton value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public void UpdateAmoInfo(int currentBullet, int maxBullet)
    {
        amoText.text = currentBullet + "/" + maxBullet;
    }

    public void AttemptToReload()
    {
        if (currentSteps > 0) currentSteps--;
        if (currentSteps <= 0) gunController.ReloadGun();
    }

    private void UpdateMusicButton()
    {
        GameBgmManager.Instance.PlayMusicToggleClipSound();
        GameBgmManager.Instance.ToggleMusic();
        int isMusicOn = GameBgmManager.Instance.isMusicOn ? 1 : 0;
        musicButtonIcon.sprite = musicBtnSprites[isMusicOn];
    }
    private void InitMusicBtn()
    {
        int isMusicOn = GameBgmManager.Instance.isMusicOn ? 1 : 0;
        musicButtonIcon.sprite = musicBtnSprites[isMusicOn];
    }
}
