using System;
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
    [SerializeField] private GameObject buttonsPanel;
    [SerializeField] private GameObject musicSettingsPanel;
    [SerializeField] private GameObject characterSettingsPanel;
    [SerializeField] private GameObject musicSettingsButton; 
    [SerializeField] private GameObject characterSettingsButton;
    [SerializeField] private GameObject onionButton;
    [SerializeField] private GameObject teemoButton;
    [SerializeField] private Image characterImage;  
    [SerializeField] private Sprite[] characterSprites;  
    [SerializeField] private GameObject changePageButton;
    [SerializeField] private GameObject musicButton;
    [SerializeField] private Image musicButtonIcon;
    [SerializeField] private Sprite[] musicBtnSprites;
    [SerializeField] private Slider volumeSlider;
    private int selectedCharacterIndex = 0;

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
        InitCharacterSelection();
        settingButton.GetComponent<Button>().onClick.AddListener(OpenSettingPanel);
        helpButton.GetComponent<Button>().onClick.AddListener(OpenHelpPanel);
        playButton.GetComponent<Button>().onClick.AddListener(OpenPlayPanel);
        rankButton.GetComponent<Button>().onClick.AddListener(OpenRankPanel);
        tutorialButton.GetComponent<Button>().onClick.AddListener(GoToTutorial);
        musicButton.GetComponent<Button>().onClick.AddListener(UpdateMusicButton);
        exitButton.GetComponent<Button>().onClick.AddListener(QuitGame);
        musicSettingsButton.GetComponent<Button>().onClick.AddListener(OpenMusicSettingsPanel);
        characterSettingsButton.GetComponent<Button>().onClick.AddListener(OpenCharacterSettingsPanel);
        changePageButton.GetComponent<Button>().onClick.AddListener(ChangePage);
        onionButton.GetComponent<Button>().onClick.AddListener(() => OnCharacterSelected(0));
        teemoButton.GetComponent<Button>().onClick.AddListener(() => OnCharacterSelected(1));
    }

    private void InitCharacterSelection()
    {
        int savedCharacterIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);
        characterImage.sprite = characterSprites[savedCharacterIndex];
        selectedCharacterIndex = savedCharacterIndex;
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
            InitSettingPanel();
            mainPanel.SetActive(false);
        }
        MainMenuSoundManager.Instance.PlayClickClipSound();
    }

    private void InitSettingPanel()
    {
        buttonsPanel.SetActive(true);
        musicSettingsPanel.SetActive(false);
        characterSettingsPanel.SetActive(false);
    }

    private void OpenRankPanel()
    {
        MainMenuSoundManager.Instance.PlayClickClipSound();
        InitPanel();
        CloseMainPanel();
        rankPanel.SetActive(true);
    }

    private void OpenCharacterSettingsPanel()
    {
        MainMenuSoundManager.Instance.PlayClickClipSound();
        buttonsPanel.SetActive(false);
        characterSettingsPanel.SetActive(true);
        musicSettingsPanel.SetActive(false);
    }

    private void OpenMusicSettingsPanel()
    {
        MainMenuSoundManager.Instance.PlayClickClipSound();
        buttonsPanel.SetActive(false);
        musicSettingsPanel.SetActive(true);
        characterSettingsPanel.SetActive(false); 
    }


    private void OnCharacterSelected(int index)
    {
        MainMenuSoundManager.Instance.PlayClickClipSound();
        selectedCharacterIndex = index;
        characterImage.sprite = characterSprites[index];
        PlayerPrefs.SetInt("SelectedCharacter", selectedCharacterIndex);
        PlayerPrefs.Save();
    }

    private void ChangePage()
    {
        if (musicSettingsPanel.activeSelf)
        {
            OpenCharacterSettingsPanel();
        }
        else if (characterSettingsPanel.activeSelf)
        {
            OpenMusicSettingsPanel();
        }
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
