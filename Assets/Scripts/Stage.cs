using UnityEngine;

public class Stage : MonoBehaviour
{
    public StageData Data;
    public GameObject Character;
    public int MaxHP = 300000;
    public int Id = 0;
    public int ATK = 5000;
    public string AnimName;
    private void Start()
    {
        if (Data != null)
        {
            Character = Data.Character;
            MaxHP = Data.MaxHP;
            Id = Data.Id;
            ATK = Data.ATK;
            AnimName = Data.AnimName;
        }
    }
}
