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
        Items = Resources.LoadAll<Item>("Items/").ToList();
        RedBonus = Resources.LoadAll<Item>("RedBonus/").ToList(); ;
        BlueBonus = Resources.LoadAll<Item>("BlueBonus/").ToList();
        GreenBonus = Resources.LoadAll<Item>("GreenBonus/").ToList();
        YellowBonus = Resources.LoadAll<Item>("YellowBonus/").ToList();
        RedPlus = Resources.Load<Item>("RedPlus");
        BluePlus = Resources.Load<Item>("BluePlus");
        GreenPlus = Resources.Load<Item>("GreenPlus");
        YellowPlus = Resources.Load<Item>("YellowPlus");
    }
}
