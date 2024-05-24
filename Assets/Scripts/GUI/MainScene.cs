using Assets.Scripts.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class MainScene : MonoBehaviour
{
    public TextMeshProUGUI CoinContent { get; private set; }
    public Button BtnExit { get; private set; }
    public Button BtnCG {  get; private set; }
    public LocalizedString LocalBtnExit;

    private void Awake()
    {
        CoinContent = transform.Find("Coin").transform.Find("CoinContent").GetComponent<TextMeshProUGUI>();
        BtnExit = transform.Find("BtnExit").GetComponent<Button>();
        BtnCG = transform.Find("BtnCG").GetComponent<Button>();
    }
    private void Start()
    {
        BtnExit.onClick.AddListener(() => { BtnExitOnClick(); });
        AudioManager.PlayMainBGM();
        TooltipManager.ShowToolTip("");
        SaveManager.LoadGame();
    }
    private void LateUpdate()
    {
        CoinContent.text = "$ " + SkillManager.Instance.Coin;
        BtnCG.interactable = GameManager.Instance.CurrentStage.Data.IsComplete;
    }

    private void BtnExitOnClick()
    {
        string content = LocalBtnExit.GetLocalizedString();
        Confirmation.ShowDioalog(content,
                () => { GameManager.Instance.ExitGame(); },
                () => { }
            );
    }
}
