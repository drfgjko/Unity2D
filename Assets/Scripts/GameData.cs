using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static bool isEndless;
    void Start() => isEndless = false;
    public void OnEndless()
    {
        isEndless = true;
    }

    public void OffEndless()
    {
        isEndless = false;
    }
    public static void InitOldGameObject()
    {
        GameObject[] effects = GameObject.FindGameObjectsWithTag("Effect");
        foreach (var effect in effects)
        {
            Destroy(effect);
        }
    }


}
