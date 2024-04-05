using Assets.Scripts.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainScene : MonoBehaviour
{
    public TextMeshProUGUI CoinContent { get; private set; }
    public Button BtnExit { get; private set; }
    public Button BtnCG {  get; private set; }

    private void Awake()
    {
        CoinContent = transform.Find("CoinContent").GetComponent<TextMeshProUGUI>();
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
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveManager.SaveGame();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            SaveManager.LoadGame();
        }
    }

    private void BtnExitOnClick()
    {
        Confirmation.ShowDioalog("是否存檔離開遊戲？",
                () => { GameManager.Instance.ExitGame(); },
                () => { }
            );
    }
}
