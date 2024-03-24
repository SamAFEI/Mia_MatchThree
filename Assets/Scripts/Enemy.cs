using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public static Enemy Instance { get; private set; }
    public TextMeshProUGUI TrunText { get; private set; }
    public TextMeshProUGUI HPText { get; private set; }
    public Slider HPSlider { get; private set; }
    public int CurrentHP { get; private set; }
    public int ATK { get; private set; }
    public bool IsDie { get { return CurrentHP <= 0; } }
    private void Awake()
    {
        Instance = this;
        HPSlider = transform.Find("HPSlider").GetComponent<Slider>();
        HPText = HPSlider.transform.Find("HPText").GameObject().GetComponent<TextMeshProUGUI>();
        TrunText = transform.Find("TrunText").GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        ATK = GameManager.Instance.CurrentStage.ATK;
        HPSlider.maxValue = 10000;
        HPSlider.maxValue = GameManager.Instance.CurrentStage.MaxHP;
        HPSlider.value = HPSlider.maxValue;
        CurrentHP = (int)HPSlider.maxValue;
    }
    private void Update()
    {
    }
    private void LateUpdate()
    {
        UpdateHPBar();
    }

    public void Hurt(int _damage)
    {
        CurrentHP = (int)Mathf.Clamp(CurrentHP - _damage, 0, HPSlider.maxValue);
    }
    public void UpdateHPBar()
    {
        float hp = Mathf.Lerp(HPSlider.value, CurrentHP, 0.1f);
        HPSlider.value = (int)hp;
        HPText.text = HPSlider.value + " / " + HPSlider.maxValue;
    }
    public void Attack()
    {
        float _damage = ATK * Random.Range(0.800f, 1.200f) * Board.Instance.DEFDown;
        Player.Instance.Hurt((int)_damage);
    }
}
