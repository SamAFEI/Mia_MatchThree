using UnityEngine;

public static class ItemDatabase 
{
    public static Item[] Items { get; private set; }
    public static Item[] RedBonus { get; private set; }
    public static Item[] BlueBonus { get; private set; }
    public static Item[] GreenBonus { get; private set; }
    public static Item[] YellowBonus { get; private set; }
    public static Item RedPlus { get; private set; }
    public static Item BluePlus { get; private set; }
    public static Item GreenPlus { get; private set; }
    public static Item YellowPlus { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        Items = Resources.LoadAll<Item>("Items/");
        RedBonus = Resources.LoadAll<Item>("RedBonus/");
        BlueBonus = Resources.LoadAll<Item>("BlueBonus/");
        GreenBonus = Resources.LoadAll<Item>("GreenBonus/");
        YellowBonus = Resources.LoadAll<Item>("YellowBonus/");
        RedPlus = Resources.Load<Item>("RedPlus");
        BluePlus = Resources.Load<Item>("BluePlus");
        GreenPlus = Resources.Load<Item>("GreenPlus");
        YellowPlus = Resources.Load<Item>("YellowPlus");
    }
}
