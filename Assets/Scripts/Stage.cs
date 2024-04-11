using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class Stage : MonoBehaviour
{
    public StageData Data;
    public GameObject Character;
    public int MaxHP = 300000;
    public int Id = 0;
    public int ATK = 5000;
    public string AnimName;
    public string StageName;
    private void Start()
    {
        /*if (Data != null)
        {
            Character = Data.Character;
            MaxHP = Data.MaxHP;
            Id = Data.Id;
            ATK = Data.ATK;
            AnimName = Data.AnimName;
        }*/
    }
    private void LateUpdate()
    {
        this.GetComponent<Button>().interactable = (Data.PriorStage == null || Data.PriorStage.IsComplete);
    }
    private void OnValidate()
    {
        if (Data != null)
        {
            Character = Data.Character;
            MaxHP = Data.MaxHP;
            Id = Data.Id;
            ATK = Data.ATK;
            AnimName = Data.AnimName;
            StageName = Data.StageName;
            gameObject.name = StageName;
        }
    }
}
