using UnityEngine;

[CreateAssetMenu(fileName = "ATK", menuName = "Match3/Skills/ATK", order = 1)]
public class ATK : BasisSkill
{
    public override void SetValue()
    {
        base.SetValue();
        SkillManager.Instance.ATK = Value;
    }
}
