using UnityEngine;

[CreateAssetMenu(fileName = "MaxHP", menuName = "Match3/Skills/MaxHP", order = 1)]
public class MaxHP : BasisSkill
{
    public override void SetValue()
    {
        base.SetValue();
        SkillManager.Instance.MaxHP = (int)Value;
    }
}
