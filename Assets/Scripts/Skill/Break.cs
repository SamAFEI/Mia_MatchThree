using UnityEngine;

[CreateAssetMenu(fileName = "Break", menuName = "Match3/Skills/Break", order = 1)]
public class Break : BasisSkill
{
    public override void DoSkill()
    {
        base.DoSkill();
        SkillManager.SetBreakSkill(true);
    }
}
