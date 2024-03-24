using UnityEngine;

[CreateAssetMenu(fileName = "Heal", menuName = "Match3/Skills/Heal", order = 1)]
public class Heal : BasisSkill
{
    public override void DoSkill()
    {
        base.DoSkill();
        Player.Instance.Hurt((int)Value * -1);
    }
}
