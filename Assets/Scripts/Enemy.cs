using System.Collections;
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
    public float HPSmooth { get; private set; }
    public float BusyTime { get; private set; }
    private void Awake()
    {
        Instance = this;
        HPSlider = transform.Find("HPSlider").GetComponent<Slider>();
        HPText = HPSlider.transform.Find("HPText").GameObject().GetComponent<TextMeshProUGUI>();
        TrunText = transform.Find("TrunText").GetComponent<TextMeshProUGUI>();
        GameManager.LoadSceneName = "";
    }
    private void Start()
    {
        ATK = GameManager.Instance.CurrentStage.ATK;
        HPSlider.maxValue = 10000;
        HPSlider.maxValue = GameManager.Instance.CurrentStage.MaxHP;
        HPSlider.value = HPSlider.maxValue;
        CurrentHP = (int)HPSlider.maxValue;
        HPText.text = HPSlider.value + " / " + HPSlider.maxValue;
    }
    private void Update()
    {
        Instance.BusyTime -= Time.deltaTime;
    }

    public void Hurt(int _damage)
    {
        AudioManager.PlayVoice(VoiceEnum.Hurt);
        if (!EnemyController.GetIsBreak1() && CurrentHP < HPSlider.maxValue / 3 * 2)
        { EnemyController.PlayAnim("Break1"); }
        else if (!EnemyController.GetIsBreak2() && CurrentHP < HPSlider.maxValue / 3 * 1)
        { EnemyController.PlayAnim("Break2"); }
        else
        { 
            EnemyController.PlayAnim("Hurt");
        }
        CurrentHP = (int)Mathf.Clamp(CurrentHP - _damage, 0, HPSlider.maxValue);
        HPSmooth = 0;
        StartCoroutine(LerpHP());
    }
    public IEnumerator LerpHP()
    {
        float smooth = 2;
        float startHP = HPSlider.value;
        while (HPSmooth < 1)
        {
            HPSmooth += Time.deltaTime * smooth;
            HPSlider.value = Mathf.Lerp(startHP, CurrentHP, HPSmooth);
            HPText.text = HPSlider.value + " / " + HPSlider.maxValue;
            yield return null;
        }
        yield return null;
    }
    public void Attack()
    {
        float damage = ATK * Random.Range(0.800f, 1.200f) * Board.Instance.DEFDown;
        EnemyController.EneymAttack((int)damage); 
    }
}
