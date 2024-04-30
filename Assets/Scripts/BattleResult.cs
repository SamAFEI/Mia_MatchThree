using TMPro;
using UnityEngine;

public class BattleResult : MonoBehaviour
{
    public static BattleResult Instance { get; private set; }
    public TextMeshProUGUI txtWin;
    public TextMeshProUGUI txtCoin;
    public TextMeshProUGUI txtCombos;
    private bool isWin;
    private void Awake()
    {
        Instance = this;
        txtWin = transform.Find("txtWin").GetComponent<TextMeshProUGUI>();
        txtCoin = transform.Find("txtCoin").GetComponent<TextMeshProUGUI>();
        txtCombos = transform.Find("txtCombos").GetComponent<TextMeshProUGUI>();
    }

    public void GameResult(bool _isWin)
    {
        isWin = _isWin;
        int _coin = (int) (Enemy.Instance.HPSlider.maxValue / 100 * (1 - Enemy.Instance.HPSlider.value / Enemy.Instance.HPSlider.maxValue));
        _coin += Board.Instance.MaxCombos * 50;
        if (isWin)
        {
            txtWin.text = "Victory";
            AudioManager.PlaySE(SEEnum.Victory);
            GameManager.CompleteStage();
        }
        else
        {
            _coin = _coin / 3;
            txtWin.text = "Failed";
            AudioManager.PlaySE(SEEnum.Failed);
        }
        txtCoin.text = "$ " + _coin;
        txtCombos.text = Board.Instance.MaxCombos + " Hits";
        SkillManager.SetCoin(_coin);
    }
    public void Continue()
    {
        SkillManager.ClearActiveSkills();
        if (isWin)
        {
            GameManager.Instance.LoadCGScene();
        }
        else
        {
            GameManager.Instance.LoadMainScene();
        }
    }
}
