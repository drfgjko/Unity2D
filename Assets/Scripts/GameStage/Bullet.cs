using UnityEngine;
using UnityEngine.SceneManagement;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb => GetComponent<Rigidbody2D>();
    void Update() => transform.up = rb.velocity;
    [SerializeField] private GameObject effectPrefab;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Target")
        {
            GameBgmManager.Instance.PlayGetScoreSound();
            Destroy(gameObject);
            Destroy(collision.gameObject);

            GameObject effect = Instantiate(effectPrefab);
            effect.transform.position = collision.bounds.center;
            float animationDuration = effect.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
            Destroy(effect, animationDuration);
            if (SceneManager.GetActiveScene().name != ("Tutorial")) UI.instance.AddScore(1);
        }
    }
}
