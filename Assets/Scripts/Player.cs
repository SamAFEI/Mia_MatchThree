using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    public TextMeshProUGUI HPText { get; private set; }
    public Slider HPSlider { get; private set; }
    public GameObject StateUI { get; private set; }
    public List<Image> StateLists { get; private set; } = new List<Image>();
    public int CurrentHP { get; private set; }
    public bool IsDie { get { return CurrentHP <= 0; } }
    private void Awake()
    {
        Instance = this;
        HPSlider = transform.Find("HPSlider").GetComponent<Slider>();
        HPText = HPSlider.transform.Find("HPText").GameObject().GetComponent<TextMeshProUGUI>();
        StateUI = transform.Find("StateUI").gameObject;
        StateLists = StateUI.GetComponentsInChildren<Image>().ToList();
    }
    private void Start()
    {
        HPSlider.maxValue = SkillManager.Instance.MaxHP;
        HPSlider.value = HPSlider.maxValue;
        CurrentHP = (int)HPSlider.maxValue;
    }
    private void LateUpdate()
    {
        UpdateHPBar();
        UpdateStateUI();
    }

    public void Hurt(int _damage)
    {
        CurrentHP = (int)Mathf.Clamp(CurrentHP - _damage, 0, HPSlider.maxValue);
    }
    public void UpdateHPBar()
    {
        float hp = Mathf.Lerp(HPSlider.value, CurrentHP, 0.1f);
        HPSlider.value = (int)hp;
        HPText.text = HPSlider.value + " / " + HPSlider.maxValue;
    }
    public void UpdateStateUI()
    {
        foreach (Image image in StateLists)
        {
            //image.color = new Color(1,1,1,0);
            image.enabled = false;
        }
        int count = 0;
        if (SkillManager.Instance.ActiveSkills.Count != 0)
        {
            for (int i = 0; i < SkillManager.Instance.ActiveSkills.Count; i++)
            {
                Skill skill = SkillManager.Instance.ActiveSkills[i];
                if (skill.DurationTime == 0) { continue; }
                StateLists[count].sprite = skill.SkillImage.sprite;
                StateLists[count].enabled = true;
                count++;
            }
        }
        if (SkillManager.Instance.ActiveDebuffs.Count != 0)
        {
            for (int i = 0; i < SkillManager.Instance.ActiveDebuffs.Count; i++)
            {
                Sprite sprite = SkillManager.Instance.ActiveDebuffs[i];
                StateLists[i + count].sprite = sprite;
                StateLists[i + count].enabled = true;
            }
        }
    }
}
