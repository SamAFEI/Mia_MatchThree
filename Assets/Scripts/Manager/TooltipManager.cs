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
    public TextMeshProUGUI Content;
    public RectTransform Background;
    private void Awake()
    {
        Content = transform.Find("Content").GetComponent<TextMeshProUGUI>();
        Background = GetComponent<RectTransform>();
    }
    private void Start()
    {
        Cursor.visible = true;
        HideToolTip();
    }
    private void Update()
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>()
                                                                , Input.mousePosition, Camera.main, out localPoint);
        transform.localPosition = localPoint;
    }
    public static void ShowToolTip(string _content)
    {
        Instance.gameObject.SetActive(true);
        Instance.Content.text = _content;
        /*float paddingSize = 4f;
        Vector2 size = new Vector2(Instance.Content.preferredWidth + paddingSize * 2
                                 , Instance.Content.preferredHeight + paddingSize * 2);
        Instance.Background.sizeDelta = size;*/
    }
    public static void HideToolTip()
    {
        Instance.gameObject.SetActive(false);
        Instance.Content.text = string.Empty;
    }
    private static void CreateDefault()
    {
        GameObject obj = Resources.Load<GameObject>("Prefabs/Manager/TooltipManager");
        obj = Instantiate(obj, Vector3.zero, Quaternion.identity, GameObject.Find("TooltipCanvas").transform);
        instance = obj.GetComponent<TooltipManager>();
    }
}
