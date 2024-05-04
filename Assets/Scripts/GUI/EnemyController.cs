using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public static EnemyController Instance { get; private set; }
    public GameObject Character { get; private set; }
    public Live2DController L2DController { get; private set; }
    public GameObject Projectile_Red;
    public GameObject Projectile_Yellow;
    public GameObject Projectile_Blue;
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

    public static void EnemyHurt(Vector3 _startPoint,int _damage, ItemColorEnum itemColor = ItemColorEnum.Red)
    {
        GameObject projectile = Instance.Projectile_Red;
        if (itemColor == ItemColorEnum.Red)
        {
            projectile = Instance.Projectile_Red;
        }
        else if (itemColor == ItemColorEnum.Blue)
        {
            projectile = Instance.Projectile_Blue;
        }
        else if (itemColor == ItemColorEnum.Yellow)
        {
            projectile = Instance.Projectile_Yellow;
        }
        GameObject Obj = Instantiate(projectile, _startPoint, Quaternion.identity);
        Obj.GetComponent<ProjectileController>().SetPoint(_startPoint, Instance.transform.position, _damage);
    }
    public static void EneymAttack(int _damage)
    {
        if (Instance.L2DController == null)
        {
            Player.Instance.SlashHurt(_damage);
        }
        else 
        {
            Instance.L2DController.SetDamage(_damage);
            PlayAnim("Attack");
        }
    }
}
