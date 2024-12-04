using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageBtnAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Button StageBtn => GetComponent<Button>();
    [SerializeField] private Animator StageBtnAnim;
    [SerializeField] private Sprite StageBtnSprite;
    [SerializeField] private GameObject StageBtnIcon;
    [SerializeField] private int StageIndex = 0;
    [SerializeField] private int StageLength = 4;
    void Start()
    {
        StageBtnIcon.GetComponent<Image>().sprite = StageBtnSprite;
        StageBtn.onClick.AddListener(LoadStage);
    }

    public void OnPointerEnter(PointerEventData eventData) => StageBtnAnim.SetBool("IsHovered", true);
    public void OnPointerExit(PointerEventData eventData) => StageBtnAnim.SetBool("IsHovered", false);
    public void LoadStage()
    {
        MainMenuSoundManager.Instance.PlayClickClipSound();
        if (StageIndex == 0)
        {
            int index = Random.Range(1, StageLength+1);
            SceneManager.LoadScene(index);
        }
        else SceneManager.LoadScene(StageIndex);

    }
}
