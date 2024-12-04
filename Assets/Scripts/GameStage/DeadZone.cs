using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
       private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Target") || collision.CompareTag("Player"))
        {
            UI.instance.OpenEndScreen();
            GameBgmManager.Instance.PlayLoseBGM();
        }
    }
}
