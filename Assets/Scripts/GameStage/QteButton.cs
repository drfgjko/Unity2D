using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class QteButton : MonoBehaviour,IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        if (SceneManager.GetActiveScene().name == "Tutorial") TutorialUI.instance.AttemptToReload();
        else UI.instance.AttemptToReload();
        GameBgmManager.Instance.PlayFinishoQteSound();
        gameObject.SetActive(false);
    }
}
