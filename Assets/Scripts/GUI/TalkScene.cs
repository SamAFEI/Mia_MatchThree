using Fungus;
using UnityEngine;
using UnityEngine.UI;

public class TalkScene : MonoBehaviour
{
    public Flowchart Chart { get; private set; }
    public Localization localization { get; private set; }
    public Stage Stage { get; private set; }
    public GameObject Dialog { get; private set; }
    public bool IsDialogHide { get; private set; }
    public Image Background { get; private set; }

    private void Awake()
    {
        Stage = GameManager.Instance.CurrentStage;
        Chart = GetComponent<Flowchart>();
        Dialog = Chart.transform.Find("SayDialog").gameObject;
        localization = GetComponent<Localization>();
        localization.SetActiveLanguage(SettingManager.Instance.Language.ToString());
        Background = GameObject.Find("Background").GetComponent<Image>();
    }
    private void Start()
    {
        Instantiate(Stage.Data.Character, Chart.transform.position, Quaternion.identity, transform);
        string _blockName = "Talk" + Stage.Data.Id;
        Chart.ExecuteIfHasBlock(_blockName);
        AudioManager.StopBGM();
        Background.sprite = Stage.Data.Background;
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
