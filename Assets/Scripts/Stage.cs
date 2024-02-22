using UnityEngine;

[CreateAssetMenu(menuName = "Match3/Stage")]
public class Stage : ScriptableObject
{
    public int value;
    public Sprite rune;
    public Sprite frame;
    public ItemColorEnum color;
    public bool isEffectH = false;
    public int level = 0; //�ثe level
    public int bonusLevel = 0; //��Tile�ɯť�
}

public enum CharaterEnum
{
    Red, Green, Blue, Yellow
}
