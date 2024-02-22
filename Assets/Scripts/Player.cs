using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    public TextMeshProUGUI HPText;
    public Image HPMax;
    public Image HPBar;
    public int CurrentHP;
    public int MaxHP;
    public bool IsDie { get { return CurrentHP <= 0; } }
    private void Awake()
    {
        Instance = this;
        HPMax = transform.Find("HPMax").GetComponent<Image>();
        HPBar = HPMax.transform.Find("HPBar").GameObject().GetComponent<Image>();
    }
    private void Start()
    {
        MaxHP = 3000;
        CurrentHP = MaxHP;
    }
    private void Update()
    {
        UpdateHPBar();
    }

    public void Hurt(int _damage)
    {
        CurrentHP = (int)Mathf.Clamp(CurrentHP - _damage, 0, MaxHP);
    }
    public void UpdateHPBar()
    {
        float hp = (float)CurrentHP / (float)MaxHP;
        HPBar.fillAmount = hp;
    }
}
