using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StageMenu : MonoBehaviour
{
    public List<Button> StageBtns = new List<Button>();
    public Button CurrentStageBtn;
    public Stage CurrentStage;

    private void Awake()
    {
        StageBtns = GetComponentsInChildren<Button>().ToList();
    }
    private void Start()
    {
        foreach (Button _btn in StageBtns)
        {
            _btn.onClick.AddListener(() => OnClick(_btn));
            if (_btn.name == GameManager.Instance.CurrentStageBtnName)
            {
                CurrentStageBtn = _btn;
                CurrentStage = _btn.GetComponent<Stage>();
            }
        }
        if (CurrentStageBtn == null)
        {
            CurrentStageBtn = StageBtns[0];
            CurrentStage = CurrentStageBtn.GetComponent<Stage>();
            GameManager.Instance.CurrentStageBtnName = CurrentStageBtn.name;
            GameManager.RegisterCurrentStage(CurrentStage);
        }
        CurrentStageBtn.transform.localScale = new Vector2(1.1f, 1.1f);
        AudioManager.PlayMainBGM();
    }

    private void OnClick(Button _sender)
    {
        foreach (Button _btn in StageBtns)
        {
            _btn.transform.localScale = new Vector2(1, 1);
        }
        CurrentStageBtn = _sender;
        CurrentStageBtn.transform.localScale = new Vector2(1.1f, 1.1f);
        CurrentStage = CurrentStageBtn.GetComponent<Stage>();
        GameManager.Instance.CurrentStageBtnName = CurrentStageBtn.name;
        GameManager.RegisterCurrentStage(CurrentStage);
    }
}
