using TMPro;
using UnityEngine;

public class MainScene : MonoBehaviour
{
    public TextMeshProUGUI CoinContent { get; private set; }

    private void Awake()
    {
        CoinContent = transform.Find("CoinContent").GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        AudioManager.PlayMainBGM();
        SkillManager.InitSkillLevel();
        TooltipManager.ShowToolTip("");
    }
    private void LateUpdate()
    {
        CoinContent.text = "$ " + SkillManager.Instance.Coin;
    }
}
