using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Confirmation : MonoBehaviour
{
    private static Confirmation instance;
    public static Confirmation Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<Confirmation>();
            }
            if (instance == null)
            {
                CreateDefault();
            }
            return instance;
        }
    }
    public TextMeshProUGUI Content { get; private set; }
    public Button BtnYes { get; private set; }
    public Button BtnCancel { get; private set; }
    public GameObject Area { get; private set; }
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Area = transform.Find("Area").gameObject;
        BtnYes = transform.Find("Area/Frame/BtnYes").GetComponent<Button>();
        BtnCancel = transform.Find("Area/Frame/BtnCancel").GetComponent<Button>();
        Content = transform.Find("Area/Frame/Content").GetComponent<TextMeshProUGUI>();
    }
    public static void ShowDioalog(string _content, Action _yesAction, Action _noAction)
    {
        Instance.Area.SetActive(true);
        Instance.Content.text = _content;
        Instance.BtnYes.onClick.RemoveAllListeners();
        Instance.BtnCancel.onClick.RemoveAllListeners();
        Instance.BtnYes.onClick.AddListener(() =>
        {
            Instance.Area.SetActive(false);
            _yesAction();
        });
        Instance.BtnCancel.onClick.AddListener(() =>
        {
            Instance.Area.SetActive(false);
            _noAction();
        });
    }
    private static void CreateDefault()
    {
        //GameObject obj = Resources.Load<GameObject>("Prefabs/Manager/Confirmation");
        //obj = Instantiate(obj, Vector3.zero, Quaternion.identity, GameObject.Find("Canvas").transform);
        GameObject obj = Resources.Load<GameObject>("Prefabs/Manager/ConfirmCanvas");
        obj = Instantiate(obj, Vector3.zero, Quaternion.identity);
        instance = obj.GetComponent<Confirmation>();
    }
}
