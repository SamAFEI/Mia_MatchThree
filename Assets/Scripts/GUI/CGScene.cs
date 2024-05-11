using Fungus;
using UnityEngine;

public class CGScene : MonoBehaviour
{
    public Flowchart Chart { get; private set; }
    public Localization localization {  get; private set; }
    public Stage Stage { get; private set; }
    public GameObject Dialog { get; private set; }
    public bool IsDialogHide { get; private set; }

    private void Awake()
    {
        Stage = GameManager.Instance.CurrentStage;
        Chart = GetComponent<Flowchart>();
        Dialog = Chart.transform.Find("SayDialog").gameObject;
        localization = GetComponent<Localization>();
        localization.SetActiveLanguage(SettingManager.Instance.Language.ToString());
    }
    private void Start()
    {
        GameObject obj = Instantiate(Stage.Data.Character, transform.position, Quaternion.identity, transform);
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
    }
}
