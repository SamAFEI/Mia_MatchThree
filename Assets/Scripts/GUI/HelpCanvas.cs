using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpCanvas : MonoBehaviour
{
    private static HelpCanvas instance;
    public static HelpCanvas Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<HelpCanvas>();
            }
            if (instance == null)
            {
                CreateDefault();
            }
            return instance;
        }
    }
    public Button BtnPrevious { get; private set; }
    public Button BtnNext { get; private set; }
    public Button BtnExit { get; private set; }
    public GameObject Area { get; private set; }
    public Image FrameImage { get; private set; }
    public int PageIndex { get; private set; }
    public List<GameObject> Pages = new List<GameObject>();
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Area = transform.Find("Area").gameObject;
        BtnPrevious = transform.Find("Area/BtnPrevious").GetComponent<Button>();
        BtnNext = transform.Find("Area/BtnNext").GetComponent<Button>();
        BtnExit = transform.Find("Area/BtnExit").GetComponent<Button>();
        FrameImage = transform.Find("Area/Frame").GetComponent<Image>();
    }
    private void Start()
    {
        for (int i = 0; i < Pages.Count; i++)
        {
            Pages[i].SetActive(false);
        }
        Pages[PageIndex].SetActive(true);
        BtnExit.onClick.AddListener(() =>
        {
            GameManager.Instance.IsHelped = true;
            Instance.Area.SetActive(false);
        });
        BtnPrevious.onClick.AddListener(() =>
        {
            if (PageIndex > 0)
            {
                Pages[PageIndex].SetActive(false);
                PageIndex--;
                Pages[PageIndex].SetActive(true);
            }
        });
        BtnNext.onClick.AddListener(() =>
        {
            if (PageIndex < Pages.Count - 1)
            {
                Pages[PageIndex].SetActive(false);
                PageIndex++;
                Pages[PageIndex].SetActive(true);
            }
        });
    }
    private void Update()
    {
        BtnPrevious.interactable = PageIndex > 0;
        BtnNext.interactable = PageIndex < (Pages.Count - 1);
    }
    public static void ShowCanvas()
    {
        Instance.Area.SetActive(true);
        GameManager.Instance.IsHelped = true;
    }

    private static void CreateDefault()
    {
        GameObject obj = Resources.Load<GameObject>("Prefabs/GUI/HelpCanvas");
        obj = Instantiate(obj, Vector3.zero, Quaternion.identity);
        instance = obj.GetComponent<HelpCanvas>();
    }
}
