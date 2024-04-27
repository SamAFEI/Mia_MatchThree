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
    public List<Sprite> PagesSprite;
    public Image FrameImage { get; private set; }
    public int PageIndex { get; private set; }
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Area = transform.Find("Area").gameObject;
        FrameImage = transform.Find("Area/Frame").GetComponent<Image>();
        BtnPrevious = transform.Find("Area/Frame/BtnPrevious").GetComponent<Button>();
        BtnNext = transform.Find("Area/Frame/BtnNext").GetComponent<Button>();
        BtnExit = transform.Find("Area/Frame/BtnExit").GetComponent<Button>();
    }
    private void Start()
    {
        //FrameImage.sprite = PagesSprite[PageIndex];
        BtnExit.onClick.AddListener(() => { Instance.Area.SetActive(false); });
        BtnPrevious.onClick.AddListener(() =>
        {
            if (PageIndex < PagesSprite.Count - 1)
            {
                PageIndex++;
                FrameImage.sprite = PagesSprite[PageIndex];
            }
        });
        BtnNext.onClick.AddListener(() =>
        {
            if (PageIndex > 0)
            {
                PageIndex--;
                FrameImage.sprite = PagesSprite[PageIndex];
            }
        });
    }
    public static void ShowCanvas()
    {
        Instance.Area.SetActive(true);
    }

    private static void CreateDefault()
    {
        GameObject obj = Resources.Load<GameObject>("Prefabs/GUI/HelpCanvas");
        obj = Instantiate(obj, Vector3.zero, Quaternion.identity);
        instance = obj.GetComponent<HelpCanvas>();
    }
}
