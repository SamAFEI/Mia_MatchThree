using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image SkillImage;
    public Animator Animator;
    private void Awake()
    {
        SkillImage = GetComponent<Image>();
        Animator = GetComponent<Animator>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        string content = string.Empty;
        if (SkillImage.enabled)
        {
            Skill skill = SkillManager.Instance.ActiveSkills.Where(x => x.SkillImage.sprite == SkillImage.sprite).SingleOrDefault();
            if (skill)
            {
                content = skill.Data.Content;
            }
            BasisSkill basisSkill = SkillManager.Instance.ActiveDebuffs.Where(x => x.Sprite == SkillImage.sprite).SingleOrDefault();
            if (basisSkill)
            {
                content = basisSkill.Content;
            }
        }
        TooltipManager.ShowToolTip(content);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.HideToolTip();
    }
}
