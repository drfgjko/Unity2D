using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    [SerializeField] private Sprite[] targetSprite;
    [SerializeField] private Sprite[] bonusSprite;
    [SerializeField] private BoxCollider2D cd => GetComponent<BoxCollider2D>();
    [SerializeField] private GameObject targetPrefab;
    [SerializeField] private GameObject bonusPrefab;
    [SerializeField] private float calmdown = 2f;
    [SerializeField] private float timer;

    private float sushiCreated;
    private float sushiMilestone = 10;
    private float bonusCalmdown = 5;
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            timer = calmdown;
            sushiCreated++;
            if (sushiCreated > sushiMilestone && calmdown > 0.5f)
            {
                sushiMilestone += 10;
                calmdown -= 0.3f;
            }
            float randomX = Random.Range(cd.bounds.min.x, cd.bounds.max.x);

            GameObject newTarget = Instantiate(targetPrefab);
            int randomIndex = Random.Range(0, targetSprite.Length);
            newTarget.transform.position = new Vector2(randomX, transform.position.y);
            newTarget.GetComponent<SpriteRenderer>().sprite = targetSprite[randomIndex];

            if (UI.instance.GetscoreValue() % bonusCalmdown == 1)
            {
                GameObject newBonus = Instantiate(bonusPrefab);
                newBonus.transform.position = new Vector2(randomX, transform.position.y);
                newBonus.GetComponent<SpriteRenderer>().sprite = bonusSprite[randomIndex];
                Destroy(newBonus, 6);
            }

        }


    }
}
