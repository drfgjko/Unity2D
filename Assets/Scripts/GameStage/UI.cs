using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public static UI instance;
    [Header("Main Details")]
    [SerializeField] private TextMeshProUGUI socreText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI amoText;
    [SerializeField] private int StageLength = 4;
    private float gameTime = 0f;
    private int scoreValue = 0;
    private int targetValue = 30;
    private bool isPaused = false;

    [Header("Panel Details")]
    [SerializeField] private GameObject successPanel;
    [SerializeField] private GameObject tryAgainBtn;
    [SerializeField] private GameObject nextBtn;
    [SerializeField] private GameObject backToMenuBtn;

    [Header("PausePanel details")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject resumeButton;
    [SerializeField] private GameObject quitButton;

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

        if (isPaused || successPanel.activeSelf || tryAgainBtn.activeSelf)
        {
            return; 
        }
        if (!isPaused)
        {
            gameTime += Time.deltaTime;
        }
        if (gameTime >= 1f)
        {
            timerText.text = gameTime.ToString("#,#");
        }
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
        musicButton.GetComponent<Button>().onClick.AddListener(UpdateMusicButton);
        quitButton.GetComponent<Button>().onClick.AddListener(QuitGame);
        backToMenuBtn.GetComponent<Button>().onClick.AddListener(QuitGame);
        tryAgainBtn.GetComponent<Button>().onClick.AddListener(RestartGame);
        nextBtn.GetComponent<Button>().onClick.AddListener(RestartGame);
    }
    private void InitPanel()
    {
        tryAgainBtn.SetActive(false);
        pausePanel.SetActive(false);
        successPanel.SetActive(false);
        Time.timeScale = 1;
        /* gameTime = 0f;
         isPaused = false;*/
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
        GameBgmManager.Instance.PlayExitClipSound();
        GameData.InitOldGameObject();
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

    public void AddScore(int value)
    {
        scoreValue += value;
        socreText.text = "x" + scoreValue.ToString("#,#");
        if (GameData.isEndless) return;
        if (scoreValue >= targetValue)
        {
            GameBgmManager.Instance.PlaySuccessBgmClip();
            OpenSuccessPanel();
        }
    }

    private void OpenSuccessPanel()
    {
        Time.timeScale = 0;
        isPaused = true;
        successPanel.SetActive(true);
        StartCoroutine(EnableButtonsWithDelay(1f));
        UpdatePlayerPrefabs();
    }


    private IEnumerator EnableButtonsWithDelay(float delay)
    {
        nextBtn.GetComponent<Button>().interactable = false;
        backToMenuBtn.GetComponent<Button>().interactable = false;
        yield return new WaitForSecondsRealtime(delay);
        nextBtn.GetComponent<Button>().interactable = true;
        backToMenuBtn.GetComponent<Button>().interactable = true;
    }

    public void UpdateAmoInfo(int currentBullet, int maxBullet)
    {
        amoText.text = currentBullet + "/" + maxBullet;
    }

    public void OpenEndScreen()
    {
        Time.timeScale = 0;
        tryAgainBtn.SetActive(true);
        UpdatePlayerPrefabs();
    }

    public void RestartGame()
    {
        int index = Random.Range(1, StageLength + 1);
        GameData.InitOldGameObject();
        SceneManager.LoadScene(index);
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

    private int GetHighestScore() { return PlayerPrefs.GetInt("HighestScore", 0); }
    private float GetLongestTime() { return PlayerPrefs.GetFloat("LongestTime", 0); }
    private void UpdatePlayerPrefabs()
    {
        if (gameTime > GetLongestTime())
        {
            PlayerPrefs.SetFloat("LongestTime", gameTime);
            PlayerPrefs.Save();
/*            Debug.Log("Longest Time: " + GetLongestTime());
*/        }
        if (scoreValue > GetHighestScore())
        {
            PlayerPrefs.SetInt("HighestScore", scoreValue);
            PlayerPrefs.Save();
/*            Debug.Log("Highest Score: " + GetHighestScore());
*/        }
    }

    public int GetscoreValue()
    {
        return scoreValue;
    }
}
