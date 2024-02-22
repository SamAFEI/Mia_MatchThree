using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StageMenu : MonoBehaviour
{
    public List<Button> StageBtns = new List<Button>();
    public Button CurrentStageBtn;

    private void Awake()
    {
        StageBtns = GetComponentsInChildren<Button>().ToList();
    }
    private void Start()
    {
        foreach (Button _btn in StageBtns)
        {
            _btn.onClick.AddListener(() => OnClick(_btn));

            if (_btn.name == GameManager.CurrentStageBtnName)
            {
                CurrentStageBtn = _btn;
            }
        }
        if (CurrentStageBtn == null)
        {
            CurrentStageBtn = StageBtns[0];
        }
        CurrentStageBtn.transform.localScale = new Vector2(1.1f, 1.1f);
    }

    private void OnClick(Button _sender)
    {
        foreach (Button _btn in StageBtns)
        {
            _btn.transform.localScale = new Vector2(1, 1);
        }
        CurrentStageBtn = _sender;
        CurrentStageBtn.transform.localScale = new Vector2(1.1f, 1.1f);
        GameManager.CurrentStageBtnName = CurrentStageBtn.name;
    }
}
