using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(200)]
public class StageMenu : MonoBehaviour
{
    public List<Button> StageBtns = new List<Button>();
    public Stage CurrentStage;

    private void Awake()
    {
        StageBtns = GetComponentsInChildren<Button>().ToList();
    }
    private void Start()
    {
        SetCurrentStage();
    }
    
    private void OnClick(Button _sender)
    {
        foreach (Button _btn in StageBtns)
        {
            _btn.transform.localScale = new Vector2(1, 1);
        }
        _sender.transform.localScale = new Vector2(1.1f, 1.1f);
        CurrentStage = _sender.GetComponent<Stage>();
        GameManager.RegisterCurrentStage(CurrentStage);
    }
    public void SetCurrentStage()
    {
        foreach (Button _btn in StageBtns)
        {
            _btn.onClick.AddListener(() => AudioManager.Instance.PlayClick());
            _btn.onClick.AddListener(() => OnClick(_btn));
            if (_btn.name == GameManager.Instance.CurrentStage.StageName)
            {
                CurrentStage = _btn.GetComponent<Stage>();
                _btn.transform.localScale = new Vector2(1.1f, 1.1f);
            }
        }
    }
}
