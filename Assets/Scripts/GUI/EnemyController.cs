using System.Collections;
using TMPro;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public static EnemyController Instance { get; private set; }
    public GameObject Character { get; private set; }
    public Live2DController L2DController { get; private set; }
    public TextMeshProUGUI HitText { get; private set; }
    public float HitVisibleTime { get; private set; }
    public float Combos { get; private set; }
    public int MaxCombos { get; private set; }
    public float Smooth { get; private set; }
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
        HitText = transform.Find("HitText").GetComponent<TextMeshProUGUI>();
        HitText.gameObject.SetActive(false);
    }
    private void Update()
    {
        Instance.HitVisibleTime -= Time.deltaTime;
        if (Instance.HitVisibleTime < 0)
        {
            HitText.gameObject.SetActive(false);
            Combos = 1;
        }
        else 
        { 
            HitText.gameObject.SetActive(true); 
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            EnemyHurt(Board.Instance.transform.position,0);
        }
    }
    public static void SetHitText(int _hit)
    {
        Instance.HitVisibleTime = 2f;
        Instance.MaxCombos = _hit;
        Instance.Smooth = 0f;
        Instance.StartCoroutine(Instance.LerpCombos());
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
    public IEnumerator LerpCombos()
    {
        float smooth = 2;
        float start = Combos;
        while (Smooth < 1)
        {
            Smooth += Time.deltaTime * smooth;
            Combos = Mathf.Lerp(start, MaxCombos, Smooth);
            HitText.text = "Combos " + (int)Combos;
            yield return null;
        }
        yield return null;
    }
}
