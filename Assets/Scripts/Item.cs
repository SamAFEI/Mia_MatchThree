using UnityEngine;

[CreateAssetMenu(menuName ="Match3/Item")]
public class Item : ScriptableObject
{
    public int value;
    public Sprite sprite;
    public ItemColorEnum color;
    public bool isEffectH = false;
    public int level = 0; //目前 level
    public int bonusLevel = 0; //給Tile升級用
}

public enum ItemColorEnum
{
    Red, Green, Blue, Yellow
}
