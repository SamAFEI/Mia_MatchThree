using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image SkillImage;
    private void Awake()
    {
        SkillImage = GetComponent<Image>();
    }
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        string content = string.Empty;
        if (SkillImage.enabled)
        {
            Skill skill = SkillManager.Instance.ActiveSkills.Where(x => x.SkillImage.sprite == SkillImage.sprite).SingleOrDefault();
            if (skill)
            {
                content = skill.Data.Content;
            }
            int index = SkillManager.Instance.DebuffIcons.IndexOf(SkillImage.sprite);
            if (index == 1)
            {
                content = "攻擊力降低";
            }
            else if (index == 2)
            {
                content = "防禦力降低";
            }
            else if (index == 3)
            {
                content = "每回合 -4000 HP";
            }
        }
        TooltipManager.ShowToolTip(content);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.HideToolTip();
    }
}
