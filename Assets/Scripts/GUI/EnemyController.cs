using Live2D.Cubism.Core;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public static EnemyController Instance { get; private set; }
    public GameObject Character { get; private set; }
    public Live2DController L2DController { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        Stage stage = GameManager.Instance.CurrentStage;
        Character = Instantiate(stage.Data.Character, transform.position, Quaternion.identity, transform);
        Character.transform.localScale *= 100;
        L2DController = Character.GetComponent<Live2DController>();
    }
    public static void PlayAnim(string _anim)
    {
        if (Instance.L2DController == null) return;
        Instance.L2DController.PlayAnim(_anim);
    }
    public static bool GetIsBreak1()
    {
        if (Instance.L2DController == null) return false;
        else return Instance.L2DController.IsBreak1;
    }
    public static bool GetIsBreak2()
    {
        if (Instance.L2DController == null) return false;
        else return Instance.L2DController.IsBreak2;
    }
}
