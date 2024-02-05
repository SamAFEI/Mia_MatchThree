using UnityEngine;

[CreateAssetMenu(menuName ="Match3/Item")]
public class Item : ScriptableObject
{
    public int value;
    public Sprite sprite;
    public ItemColorEnum color;
    public bool isEffectH = false;
    public int level = 0; //�ثe level
    public int bonusLevel = 0; //��Tile�ɯť�
}

public enum ItemColorEnum
{
    Red, Green, Blue, Yellow
}
