using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Skill : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public BasisSkill Data;
    public int ColdDownTime { get; private set; }
    public int DurationTime { get; private set; }
    public TextMeshProUGUI CDContent { get; private set; }
    public Button Button { get; private set; }
    public Image SkillImage { get; private set; }
    public TextMeshProUGUI SkillName { get; private set; }
    public TextMeshProUGUI SkillContent { get; private set; }
    public bool IsBattleScene { get { return SceneManager.GetActiveScene().name == "MatchThreeScene"; } }
    private static LTDescr DelayTime;
    private string content;
    private void Awake()
    {
        Button = transform.Find("SkillButton").GetComponent<Button>();
        CDContent = Button.transform.Find("CDContent").GetComponent<TextMeshProUGUI>();
        SkillImage = transform.Find("SkillButton").GetComponent<Image>();
        SkillName = transform.Find("SkillName").GetComponent<TextMeshProUGUI>();
        SkillContent = transform.Find("SkillContent").GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        Button.onClick.AddListener(() => { ClickButton(); });
        SkillImage.sprite = Data.Sprite; 
        CDContent.text = "";
        //SkillName.text = Data.Name;
    }
    private void LateUpdate()
    {
        UpdateContent();
        if (IsBattleScene)
        {
            SkillContent.text = "";
            CDContent.text = "";
            if (ColdDownTime > 0)
            {
                CDContent.text = ColdDownTime.ToString();
            }
            Button.enabled = ColdDownTime == 0 && !Board.Instance.IsBusy;
        }
        else
        {
            Button.interactable = !(Data.Level == Data.MaxLevel || Data.Cost > SkillManager.Instance.Coin);
        }
    }
    public void UpLevel()
    {
        SkillManager.SetCoin(Data.Cost * -1);
        Data.UpLevel(); 
        AudioManager.PlaySE(SEEnum.LevelUp);
    }
    public void DoSkill()
    {
        DurationTime = Data.Duration;
        ColdDownTime = Data.ColdDown;
        Data.DoSkill();
        SkillManager.AddActiveSkills(this);
    }
    public void UpdateSkillTime()
    {
        DurationTime = Mathf.Clamp(DurationTime - 1, 0, Data.Duration);
        ColdDownTime = Mathf.Clamp(ColdDownTime - 1, 0, Data.ColdDown);
        if (DurationTime == 0)
        {
            Data.FinishSkill();
        }
        if (ColdDownTime == 0)
        {
            SkillManager.RemoveActiveSkills(this);
        }
    }
    public void UpdateContent()
    {
        SkillContent.text = "LV " + Data.Level;
        if (!IsBattleScene && Data.Level < Data.MaxLevel)
        {
            SkillContent.text = SkillContent.text + " $" + Data.Cost;
        }

        content = Data.Content;
        if (!IsBattleScene)
        {
            if (content != "") { content += "\n"; }
            content += "Level UP $" + Data.Cost + "\n" + Data.LeveContent;
        }
    }
    public void ClickButton()
    {
        string content;
        content = Data.Content;
        if (content == "") { content = Data.Name; }
        if (IsBattleScene)
        {
            Confirmation.ShowDioalog(content,
                () => { DoSkill(); },
                () => { }
            );
        }
        else
        {
            content = "Level Up $" + Data.Cost + "\n" + Data.LeveContent;
            Confirmation.ShowDioalog(content, 
                () => { UpLevel(); },
                () => { }
            );
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        DelayTime = LeanTween.delayedCall(0.5f, () =>
        {
            TooltipManager.ShowToolTip(content, Data.Name);
        });
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.cancel(DelayTime.uniqueId);
        TooltipManager.HideToolTip();
    }

}
