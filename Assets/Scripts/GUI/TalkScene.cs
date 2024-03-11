using Fungus;
using UnityEngine;

public class TalkScene : MonoBehaviour
{
    public Flowchart Dialog { get; private set; }
    public Localization localization { get; private set; }
    public Stage Stage { get; private set; }

    private void Awake()
    {
        Stage = GameManager.Instance.CurrentStage;
        Dialog = GetComponent<Flowchart>(); 
        localization = GetComponent<Localization>();
        localization.SetActiveLanguage(SettingManager.Instance.Language.ToString());
    }
    private void Start()
    {
        Instantiate(Stage.Character, transform.position, Quaternion.identity);
        string _blockName = "Talk" + Stage.Id;
        Dialog.ExecuteIfHasBlock(_blockName);
        AudioManager.StopBGM();
    }
}
