using UnityEngine;

[CreateAssetMenu(fileName = "Power", menuName = "Match3/Skills/Power", order = 1)]
public class Power : BasisSkill
{
    public override void DoSkill()
    {
        base.DoSkill();
        SkillManager.Instance.Power = Value;
    }
    public override void FinishSkill() 
    { 
        base.FinishSkill();
        SkillManager.Instance.Power = 1;
    }
}
