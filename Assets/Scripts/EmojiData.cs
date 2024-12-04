using UnityEngine;

[CreateAssetMenu(fileName = "EmojiData", menuName = "ScriptableObjects/EmojiData", order = 1)]
public class EmojiData : ScriptableObject
{
    public GameObject emojiPrefab;
    public Sprite[] emojiSprites;
}