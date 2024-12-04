using UnityEngine;

public class Bonus : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameBgmManager.Instance.PlayGetBonusSound();
            GunController.instance.superPower = true;
            GunController.instance.timer = GunController.instance.calmdown;
            SkillManager.instance.ReloadSkill();
            Destroy(gameObject);
        }
    }
}
