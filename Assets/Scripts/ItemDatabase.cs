using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ItemDatabase
{
    public static List<Item> Items { get; private set; } = new List<Item>();
    public static List<Item> RedBonus { get; private set; } = new List<Item>();
    public static List<Item> BlueBonus { get; private set; } = new List<Item>();
    public static List<Item> GreenBonus { get; private set; } = new List<Item>();
    public static List<Item> YellowBonus { get; private set; } = new List<Item>();
    public static Item RedPlus { get; private set; }
    public static Item BluePlus { get; private set; }
    public static Item GreenPlus { get; private set; }
    public static Item YellowPlus { get; private set; }
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        Items = Resources.LoadAll<Item>("Items/Base/").ToList();
        RedBonus = Resources.LoadAll<Item>("Items/RedBonus/").ToList();
        BlueBonus = Resources.LoadAll<Item>("Items/BlueBonus/").ToList();
        GreenBonus = Resources.LoadAll<Item>("Items/GreenBonus/").ToList();
        YellowBonus = Resources.LoadAll<Item>("Items/YellowBonus/").ToList();
        RedPlus = Resources.Load<Item>("Items/Plus/RedPlus");
        BluePlus = Resources.Load<Item>("Items/Plus/BluePlus");
        GreenPlus = Resources.Load<Item>("Items/Plus/GreenPlus");
        YellowPlus = Resources.Load<Item>("Items/Plus/YellowPlus");
    }
}
