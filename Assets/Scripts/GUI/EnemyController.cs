using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public static EnemyController Instance { get; private set; }
    public GameObject Character { get; private set; }
    public Live2DController L2DController { get; private set; }
    public GameObject Projectile;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        Stage stage = GameManager.Instance.CurrentStage;
        Character = Instantiate(stage.Data.Character, transform.position, Quaternion.identity, transform);
        Character.name = "Live2DEnemy";
        Character.transform.localScale *= 100;
        L2DController = Character.GetComponent<Live2DController>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            EnemyHurt(Board.Instance.transform.position,0);
        }
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

    public static void EnemyHurt(Vector3 _startPoint,int _damage)
    {
        GameObject Obj = Instantiate(Instance.Projectile, _startPoint, Quaternion.identity);
        Obj.GetComponent<ProjectileController>().SetPoint(_startPoint, Instance.transform.position, _damage);
    }
}
