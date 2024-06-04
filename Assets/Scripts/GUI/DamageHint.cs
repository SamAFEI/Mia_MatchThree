using TMPro;
using UnityEngine;

public class DamageHint : MonoBehaviour
{
    public int Damage;
    public TextMeshProUGUI Content;
    public Color color;
    public float Alpha = 1;
    public float Speed = 5;
    public float SpeedWeak = 0.1f;
    public float WeakTime = 0.5f;
    public float DisTime = 3;

    private void Awake()
    {
        Content = GetComponent<TextMeshProUGUI>();
        Destroy(gameObject, DisTime);
        color = Content.color;
    }
    private void Start()
    {
        Content.text = Damage + "";
    }

    private void Update()
    {
        transform.Translate(0, Speed * Time.deltaTime, 0);
        if (Speed > 0)
        {
            Speed -= SpeedWeak;
        }
        else if (Speed <= 0 && WeakTime > 0)
        {
            WeakTime -= Time.deltaTime;
        }
        else if (Alpha > 0 && WeakTime <= 0)
        {
            Alpha -= 0.01f;
        }
        Content.color = new Color(color.r, color.g, color.b, Alpha);
    }
}
