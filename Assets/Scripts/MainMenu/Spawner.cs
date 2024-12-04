using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spawner : MonoBehaviour
{
    [SerializeField] private List<EmojiData> emojiDataList;
    [SerializeField] private BoxCollider2D cd => GetComponent<BoxCollider2D>();

    private float menuCalmdown = 1.5f;
    private float tutorialcalmdown = 1f;
    private float timer;
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            if(SceneManager.GetActiveScene().name == "Tutorial") InitTimer(tutorialcalmdown);
            else InitTimer(menuCalmdown);
            int randomIndex = Random.Range(0, emojiDataList.Count);
            EmojiData selectedEmojiData = emojiDataList[randomIndex];
            GameObject newTarget = Instantiate(selectedEmojiData.emojiPrefab);
            Sprite[] selectedSpriteArray = selectedEmojiData.emojiSprites;

            float randomX = Random.Range(cd.bounds.min.x, cd.bounds.max.x);
            int randomSpriteIndex = Random.Range(0, selectedSpriteArray.Length);
            newTarget.transform.position = new Vector2(randomX, transform.position.y);
            newTarget.GetComponent<SpriteRenderer>().sprite = selectedSpriteArray[randomSpriteIndex];
        }
    }

    private void InitTimer(float calmdown)
    {
        timer = calmdown;
    }
}
