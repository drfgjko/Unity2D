using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;
    [SerializeField] private GameObject effectPrefab;
    private Animator Anim => GetComponent<Animator>();

    private int skillNum = 3;

    private void Awake() => instance = this;
    void Start()
    {
        if (GameData.isEndless || SceneManager.GetActiveScene().name == "Tutorial") gameObject.SetActive(true);
        else gameObject.SetActive(false);
        Anim.SetInteger("Num", skillNum);
    }

    void Update()
    {
        if (skillNum > 0 && Input.GetKeyDown(KeyCode.R))
        {
            GameBgmManager.Instance.PlayFinishoQteSound();
            skillNum--;
            CastSkill();
        }
        AnimationController();
    }
    private void AnimationController()
    {
        Anim.SetInteger("Num", skillNum);
    }
    private void CastSkill()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Target");
        foreach (GameObject target in targets)
        {
            Destroy(target);
            GameObject effect = Instantiate(effectPrefab);
            effect.transform.position = target.GetComponent<Collider2D>().bounds.center;
            float animationDuration = effect.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
            Destroy(effect, animationDuration);
        }
    }

    public void ReloadSkill()
    {
        skillNum = 3;
    }
}
