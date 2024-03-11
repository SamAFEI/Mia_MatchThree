using TMPro;
using UnityEngine;

public class BattleResult : MonoBehaviour
{
    public static BattleResult Instance { get; private set; }
    public TextMeshProUGUI txtWin;
    private bool isWin;
    private void Awake()
    {
        Instance = this;
        txtWin = transform.Find("txtWin").GetComponentInChildren<TextMeshProUGUI>();
    }

    public void GameResult(bool _isWin)
    {
        isWin = _isWin;
        if (isWin)
        {
            txtWin.text = "Victory";
        }
        else
        {
            txtWin.text = "Failed";
        }
    }
    public void Continue()
    {
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
