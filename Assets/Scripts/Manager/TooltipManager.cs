using TMPro;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    private static TooltipManager instance;
    public static TooltipManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<TooltipManager>();
            }
            if (instance == null)
            {
                CreateDefault();
            }
            return instance;
        }
    }
    public TextMeshProUGUI Header;
    public TextMeshProUGUI Content;
    public RectTransform Background;
    public RectTransform MyRectTransform;
    public Canvas MyCanvas;
    public GameObject DamageHint;
    private void Awake()
    {
        MyRectTransform = GetComponent<RectTransform>();
        MyCanvas = GetComponent<Canvas>();
        Background = transform.Find("Background").GetComponent<RectTransform>();
        Header = Background.Find("Header").GetComponent<TextMeshProUGUI>();
        Content = Background.Find("Content").GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        MyCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        MyCanvas.worldCamera = Camera.main;
        Cursor.visible = true;
        HideToolTip();
    }
    private void Update()
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(Instance.MyRectTransform
                                                                , Input.mousePosition, Camera.main, out localPoint);
        Vector2 anchoredPosition = Instance.Background.anchoredPosition;
        float pivotX = 0;
        float pivotY = 0;
        if (Instance.Background.rect.width + anchoredPosition.x > Instance.MyRectTransform.rect.width)
        {
            pivotX = 1;
        }
        if (Instance.Background.rect.height + anchoredPosition.y > Instance.MyRectTransform.rect.height)
        {
            pivotX = 1;
            pivotY = 1;
        }
        Instance.Background.pivot = new Vector2(pivotX, pivotY);
        Instance.Background.localPosition = localPoint;
    }
    public static void ShowToolTip(string _content, string _header = "")
    {
        Instance.Background.gameObject.SetActive(true);
        Instance.Header.gameObject.SetActive(!string.IsNullOrEmpty(_header));
        Instance.Header.text = _header;
        Instance.Content.text = _content;
        /*float paddingSize = 4f;
        Vector2 size = new Vector2(Instance.Content.preferredWidth + paddingSize * 2
                                 , Instance.Content.preferredHeight + paddingSize * 2);
        Instance.Background.sizeDelta = size;*/
    }
    public static void HideToolTip()
    {
        Instance.Background.gameObject.SetActive(false);
        Instance.Header.text = string.Empty;
        Instance.Content.text = string.Empty;
    }
    private static void CreateDefault()
    {
        GameObject obj = Resources.Load<GameObject>("Prefabs/GUI/TooltipCanvas");
        obj = Instantiate(obj, Vector3.zero, Quaternion.identity);
        instance = obj.GetComponent<TooltipManager>();
    }
    public static void SpawnDamageHint(Vector3 _spawnPoint, int _damage)
    {
        GameObject obj = Instantiate(Instance.DamageHint, _spawnPoint, Quaternion.identity, Instance.transform);
        obj.GetComponent<DamageHint>().Damage = _damage;
    }
    public static void SpawnDamageHint(Vector3 _spawnPoint, int _damage, Color _color)
    {
        GameObject obj = Instantiate(Instance.DamageHint, _spawnPoint, Quaternion.identity, Instance.transform);
        obj.GetComponent<DamageHint>().Damage = _damage;
        obj.GetComponent<DamageHint>().color = _color;
    }
}
