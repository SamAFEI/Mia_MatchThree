using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "Match3/StageData", order = 1)]
public class StageData : ScriptableObject
{
    public GameObject Character;
    public int MaxHP = 300000;
    public int Id = 0;
    public int ATK = 5000;
    public string AnimName;
    public string StageName;
    public bool IsComplete;
    public StageData PriorStage;
}
