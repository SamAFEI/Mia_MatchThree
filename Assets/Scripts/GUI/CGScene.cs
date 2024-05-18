using Fungus;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CGScene : MonoBehaviour
{
    public Flowchart Chart { get; private set; }
    public Localization localization { get; private set; }
    public Stage Stage { get; private set; }
    public GameObject Dialog { get; private set; }
    public bool IsDialogHide { get; private set; }
    public int CGIndex { get; private set; }
    public Image CGFrame { get; private set; }
    public List<Sprite> CGLists { get; private set; } = new List<Sprite>();

    private void Awake()
    {
        CGFrame = GameObject.Find("Canvas").transform.Find("CGFrame").GetComponent<Image>();
        Stage = GameManager.Instance.CurrentStage;
        Chart = GetComponent<Flowchart>();
        Dialog = Chart.transform.Find("SayDialog").gameObject;
        localization = GetComponent<Localization>();
        localization.SetActiveLanguage(SettingManager.Instance.Language.ToString());
        if (GameManager.Instance.IsMosaic)
        { CGLists.AddRange(Stage.Data.MosaicCGList); }
        else
        { CGLists.AddRange(Stage.Data.CGList); }
    }
    private void Start()
    {
        //GameObject obj = Instantiate(Stage.Data.Character, transform.position, Quaternion.identity, transform);

        CGIndex = -1;
        NextCGIndex();
        string _blockName = "CG" + Stage.Data.Id;
        Chart.ExecuteIfHasBlock(_blockName);
        AudioManager.StopBGM();
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && !IsDialogHide)
        {
            Dialog.GetComponent<DialogInput>().SetNextLineFlag();
        }
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            IsDialogHide = !IsDialogHide;
        }
        if (IsDialogHide)
        {
            Dialog.GetComponent<SayDialog>().GetComponent<CanvasGroup>().alpha = 0f;
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            NextCGIndex();
        }
    }

    private void NextCGIndex()
    {
        CGIndex++;
        Debug.Log(CGIndex);
        CGFrame.sprite = CGLists[CGIndex];
    }
}
