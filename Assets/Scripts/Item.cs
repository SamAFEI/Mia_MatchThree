using UnityEngine;

[CreateAssetMenu(menuName ="Match3/Item")]
public class Item : ScriptableObject
{
    public int value;
    public Sprite rune;
    public Sprite frame;
    public ItemColorEnum color;
    public bool isEffectH = false;
    public int level = 0; //目前 level
    public int bonusLevel = 0; //給Tile升級用
    public ItemDebuffEnum debuffIndex = ItemDebuffEnum.Non;
    public Material GlowMaterial;

    private void OnValidate()
    {
        if (color == ItemColorEnum.Red)
            GlowMaterial = Resources.Load<Material>("Material/TileMAT/RedGlowMAT");
        else if (color == ItemColorEnum.Green)
            GlowMaterial = Resources.Load<Material>("Material/TileMAT/GreenGlowMAT");
        else if (color == ItemColorEnum.Blue)
            GlowMaterial = Resources.Load<Material>("Material/TileMAT/BlueGlowMAT");
        else if (color == ItemColorEnum.Yellow)
            GlowMaterial = Resources.Load<Material>("Material/TileMAT/YellowGlowMAT");
    }
}

public enum ItemColorEnum
{
    Red, Green, Blue, Yellow
}
public enum ItemDebuffEnum
{
    Non, ATKDown, DEFDown, Poison
}
