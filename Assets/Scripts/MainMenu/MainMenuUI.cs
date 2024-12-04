using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public static MainMenuUI instance;

    [Header("MainPanel details")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private GameObject helpPanel;
    [SerializeField] private GameObject playPanel;
    [SerializeField] private GameObject rankPanel;
    [SerializeField] private GameObject exitButton;
    [SerializeField] private GameObject settingButton;
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject helpButton;
    [SerializeField] private GameObject rankButton;

    [Header("SettingBtn details")]
    [SerializeField] private GameObject musicButton;
    [SerializeField] private Image musicButtonIcon;
    [SerializeField] private Sprite[] musicBtnSprites;
    [SerializeField] private Slider volumeSlider;

    [Header("HelpBtn details")]
    [SerializeField] private GameObject tutorialButton;

    [Header("RankPanel details")]
    [SerializeField] private TextMeshProUGUI socreText;
    [SerializeField] private TextMeshProUGUI timeText;

    [Header("Cursor details")]
    [SerializeField] private Texture2D customCursorTexture;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        volumeSlider.value = 100;
        InitPanel();
        InitCursor();
        InitMusicBtn();
        InitializePlayerPrefs();
        InitBestScore(); 
        settingButton.GetComponent<Button>().onClick.AddListener(OpenSettingPanel);
        helpButton.GetComponent<Button>().onClick.AddListener(OpenHelpPanel);
        playButton.GetComponent<Button>().onClick.AddListener(OpenPlayPanel);
        rankButton.GetComponent<Button>().onClick.AddListener(OpenRankPanel);
        tutorialButton.GetComponent<Button>().onClick.AddListener(GoToTutorial);
        musicButton.GetComponent<Button>().onClick.AddListener(UpdateMusicButton);
        exitButton.GetComponent<Button>().onClick.AddListener(QuitGame);
    }

    private void InitBestScore()
    {
        socreText.text = PlayerPrefs.GetInt("HighestScore", 0).ToString();
        if (PlayerPrefs.GetFloat("LongestTime") < 1) timeText.text = "0";
        else timeText.text = PlayerPrefs.GetFloat("LongestTime").ToString("#,#");
    }

    private void InitPanel()
    {
        mainPanel.SetActive(true);
        settingPanel.SetActive(false);
        helpPanel.SetActive(false);
        playPanel.SetActive(false);
        rankPanel.SetActive(false);
        Time.timeScale = 1;
    }

    private void InitializePlayerPrefs()
    {
        if (!PlayerPrefs.HasKey("HasLaunchedBefore"))
        {
            PlayerPrefs.SetInt("HasLaunchedBefore", 1);

            PlayerPrefs.SetInt("HighestScore", 0);
            PlayerPrefs.SetFloat("LongestTime", 0f);

            PlayerPrefs.Save();

            Debug.Log("First Time");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) ResumeGame();
    }
    private void CloseMainPanel()
    {
        mainPanel.SetActive(false);
    }

    private void OpenHelpPanel()
    {
        MainMenuSoundManager.Instance.PlayClickClipSound();
        InitPanel();
        CloseMainPanel();
        helpPanel.SetActive(true);
    }

    private void GoToTutorial()
    {
        MainMenuSoundManager.Instance.PlayClickClipSound();
        GameData.InitOldGameObject();
        SceneManager.LoadScene("Tutorial");
    }

    private void OpenPlayPanel()
    {
        MainMenuSoundManager.Instance.PlayClickClipSound();
        InitPanel();
        CloseMainPanel();
        playPanel.SetActive(true);
    }

    private void QuitGame()
    {
        MainMenuSoundManager.Instance.PlayClickClipSound();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
Application.Quit();
#endif

    }

    private void OpenSettingPanel()
    {
        if (!settingPanel.activeSelf)
        {
            settingPanel.SetActive(true);
            mainPanel.SetActive(false);
        }
        MainMenuSoundManager.Instance.PlayClickClipSound();
    }

    private void OpenRankPanel()
    {
        MainMenuSoundManager.Instance.PlayClickClipSound();
        InitPanel();
        CloseMainPanel();
        rankPanel.SetActive(true);
    }


    public void ResumeGame()
    {
        if (settingPanel.activeSelf || playPanel.activeSelf || helpPanel.activeSelf || rankPanel.activeSelf)
        {
            MainMenuSoundManager.Instance.PlayResumeClipSound();
            InitPanel();
        }
    }

    private void UpdateMusicButton()
    {
        MainMenuSoundManager.Instance.PlayClickClipSound();
        MainMenuSoundManager.Instance.ToggleMusic();
        int isMusicOn = MainMenuSoundManager.Instance.isMusicOn ? 1 : 0;
        musicButtonIcon.sprite = musicBtnSprites[isMusicOn];
    }

    private void InitMusicBtn()
    {
        int isMusicOn = MainMenuSoundManager.Instance.isMusicOn ? 1 : 0;
        musicButtonIcon.sprite = musicBtnSprites[isMusicOn];
    }
    private void InitCursor() => Cursor.SetCursor(customCursorTexture, Vector2.zero, CursorMode.Auto);
}
