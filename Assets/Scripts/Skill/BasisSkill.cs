using System.Collections.Generic;
using UnityEngine;

public class BasisSkill : ScriptableObject
{
    public Sprite Sprite;
    public string Name;
    [TextArea]
    public string Content;
    [TextArea]
    public string LeveContent;
    public int Level;
    public int MaxLevel = 2;
    [SerializeField] private List<float> Values = new List<float>();
    [SerializeField] private List<int> Costs = new List<int>();
    [SerializeField] private List<int> ColdDowns = new List<int>();
    [SerializeField] private List<int> Durations = new List<int>();
    public float Value { get { return Values[Level]; } }
    public int ColdDown { get { return ColdDowns[Level]; } }
    public int Duration { get { return Durations[Level]; } }
    public int Cost { get { return Costs[Level]; } }

    public virtual void SetValue() { }
    public virtual void DoSkill() { }
    public virtual void FinishSkill() { }
    public virtual void UpLevel()
    {
        Level++;
        SetValue();
    }
}
