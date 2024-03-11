using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    public TextMeshProUGUI HPText { get; private set; }
    public Slider HPSlider { get; private set; }
    public int CurrentHP { get; private set; }
    public bool IsDie { get { return HPSlider.value <= 0; } }
    private void Awake()
    {
        Instance = this;
        HPSlider = transform.Find("HPSlider").GetComponent<Slider>();
        HPText = HPSlider.transform.Find("HPText").GameObject().GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        HPSlider.maxValue = 3000;
        HPSlider.value = HPSlider.maxValue;
        CurrentHP = (int)HPSlider.maxValue;
    }
    private void Update()
    {
        UpdateHPBar();
    }

    public void Hurt(int _damage)
    {
        CurrentHP = (int)Mathf.Clamp(CurrentHP - _damage, 0, HPSlider.maxValue);
    }
    public void UpdateHPBar()
    {
        float hp = CurrentHP;
        hp = Mathf.Lerp(HPSlider.value, hp, 0.1f);
        HPSlider.value = (int)hp;
        HPText.text = HPSlider.value + " / " + HPSlider.maxValue;
    }
}
