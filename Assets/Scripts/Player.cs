using System.Collections;
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
    public Sprite FrameSprite { get; private set; }
    public int CurrentHP { get; private set; }
    public bool IsDie { get { return CurrentHP <= 0; } }
    public float HPSmooth { get; private set; }
    public GameObject HealFX;
    public GameObject SlashFX;
    public GameObject Object;
    private void Awake()
    {
        Instance = this;
        HPSlider = transform.Find("HPSlider").GetComponent<Slider>();
        HPText = HPSlider.transform.Find("HPText").GameObject().GetComponent<TextMeshProUGUI>();
        StateUI = transform.Find("StateUI").gameObject;
        StateLists = StateUI.GetComponentsInChildren<Image>().ToList();
        FrameSprite = Resources.Load<Sprite>("Sprites/Skills/Frame");
    }
    private void Start()
    {
        HPSlider.maxValue = SkillManager.Instance.MaxHP;
        HPSlider.value = HPSlider.maxValue;
        CurrentHP = (int)HPSlider.maxValue;
        HPText.text = HPSlider.value + " / " + HPSlider.maxValue;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            Hurt(0);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            SlashHurt(0);
        }
        UpdateStateUI();
    }
    public void SlashHurt(int _damage)
    {
        //Debug.Log(Object.transform.position);
        float length = 2.5f;
        float startX = Random.Range(-8f, -2.5f);
        float endX = startX >= -5.5f ? startX - length : startX + length;
        Vector3 startPoint = new Vector3(startX, -2.5f, 90f);
        Vector3 endPoint = new Vector3(endX, -5, 90f);
        GameObject Obj = Instantiate(SlashFX, startPoint, Quaternion.identity);
        Obj.GetComponent<SlashController>().SetPoint(startPoint, endPoint, _damage);
        StartCoroutine(GameManager.ShakeCamera(0.15f, 0.4f));
        Debug.Log("SlashHurt");
    }
    public void Hurt(int _damage)
    {
        CurrentHP = (int)Mathf.Clamp(CurrentHP - _damage, 0, HPSlider.maxValue);
        HPSmooth = 0;
        StartCoroutine(LerpHP());
    }
    private IEnumerator LerpHP()
    {
        float smooth = 2;
        float startHP = HPSlider.value; 
        if (CurrentHP >= startHP) //Heal
        {
            AudioManager.PlaySE(SEEnum.Heal);
            GameObject Obj = Instantiate(Instance.HealFX, HPSlider.transform.position, Quaternion.identity);
            Destroy(Obj,1f);
        }
        while (HPSmooth < 1)
        {
            HPSmooth += Time.deltaTime * smooth;
            HPSlider.value = Mathf.Lerp(startHP, CurrentHP, HPSmooth);
            HPText.text = HPSlider.value + " / " + HPSlider.maxValue;
            yield return null;
        }
        yield return null;
    }
    public void UpdateStateUI()
    {
        List<BasisSkill> activeList = new List<BasisSkill>();
        Skill skill = SkillManager.Instance.ActiveSkills.FirstOrDefault(x => x.DurationTime > 0);
        if (skill != null)
        {
            activeList.Add(skill.Data);
        }
        activeList.AddRange(SkillManager.Instance.ActiveDebuffs);
        for (int i = 0; i < StateLists.Count; i++)
        {
            Animator animator = StateLists[i].gameObject.GetComponent<Animator>();
            SpriteRenderer spriteRenderer = StateLists[i].gameObject.GetComponent<SpriteRenderer>();
            if (i < activeList.Count)
            {
                if (!animator.enabled || activeList[i].name != StateLists[i].name)
                {
                    StateLists[i].sprite = activeList[i].Sprite;
                    StateLists[i].enabled = true;
                    bool animEnabled = System.Enum.GetNames(typeof(ItemDebuffEnum)).FirstOrDefault(x => x == activeList[i].name) != null;
                    spriteRenderer.enabled = animEnabled;
                    animator.enabled = animEnabled;
                    if (animator.enabled)
                    {
                        animator.Play(StateLists[i].sprite.name);
                    }
                }
            }
            else
            {
                StateLists[i].sprite = null;
                StateLists[i].enabled = false;
                animator.Play("NoState");
                spriteRenderer.enabled = false;
                animator.enabled = false;
            }
        }
    }
}
