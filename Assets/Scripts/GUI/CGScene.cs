using Fungus;
using UnityEngine;

public class CGScene : MonoBehaviour
{
    public Flowchart Dialog { get; private set; }
    public Localization localization {  get; private set; }
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
        GameObject obj = Instantiate(Stage.Data.Character, transform.position, Quaternion.identity, transform);
        string _blockName = "CG" + Stage.Data.Id;
        Dialog.ExecuteIfHasBlock(_blockName);
        AudioManager.StopBGM();
    }
}
