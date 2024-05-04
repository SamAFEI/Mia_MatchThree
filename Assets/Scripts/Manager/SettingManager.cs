using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    private static SettingManager instance;
    public static SettingManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<SettingManager>();
            }
            if (instance == null)
            {
                CreateDefault();
            }
            return instance;
        }
    }
    public Resolution Resolution { get; private set; }
    public bool IsFullScreen { get; private set; }
    public LanguageEnum Language { get; private set; }
    public List<Resolution> Resolutions { get; private set; }
    public GameObject Area { get; private set; }
    public TMP_Dropdown ResolutionsDropdown { get; private set; }
    public TMP_Dropdown LanguageDropdown { get; private set; }
    public Toggle FullScreenToggle { get; private set; }
    public List<string> LanguageList { get; private set; } = new List<string>();
    public Slider BGMVolumeSlider { get; private set; }
    public Slider SEVolumeSlider { get; private set; }
    public Slider VoiceVolumeSlider { get; private set; }
    private void Awake()
    {
        //Debug.Log(Application.systemLanguage.ToString());
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        //Instance = this;
        DontDestroyOnLoad(this);
        Area = transform.Find("Area").gameObject;
        ResolutionsDropdown = transform.Find("Area/Menu/ResolutionsDropdown").GetComponent<TMP_Dropdown>();
        LanguageDropdown = transform.Find("Area/Menu/LanguageDropdown").GetComponent<TMP_Dropdown>();
        FullScreenToggle = transform.Find("Area/Menu/FullScreenToggle").GetComponent<Toggle>();
        BGMVolumeSlider = transform.Find("Area/Menu/BGMVolumeSlider").GetComponent<Slider>();
        SEVolumeSlider = transform.Find("Area/Menu/SEVolumeSlider").GetComponent<Slider>();
        VoiceVolumeSlider = transform.Find("Area/Menu/VoiceVolumeSlider").GetComponent<Slider>();
        SetAreaActive(false);
    }
    private void Start()
    {
#if !UNITY_WEBPLAYER
        SetResolutionDropdown();
        FullScreenToggle.isOn = IsFullScreen;
#endif
        SetLanguageDropdown();
        SetAudioVolumeSlider();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetAreaActive(!Area.activeSelf);
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GameManager.Instance.LoadTitleScene();
        }
    }
    private static void CreateDefault()
    {
        GameObject obj = Resources.Load<GameObject>("Prefabs/Manager/SettingManager");
        obj = Instantiate(obj, Vector3.zero, Quaternion.identity);
        instance = obj.GetComponent<SettingManager>();
        Instance.Resolution = Screen.currentResolution;
        Instance.IsFullScreen = Screen.fullScreen;
        Instance.Language = LanguageEnum.EN;
        if (Application.systemLanguage == SystemLanguage.ChineseTraditional)
        {
            Instance.Language = LanguageEnum.ZH;
        }
        else if (Application.systemLanguage == SystemLanguage.Japanese)
        {
            Instance.Language = LanguageEnum.JP;
        }
    }
    public static void SetAreaActive(bool _value)
    {
        Instance.Area.SetActive(_value);
    }
    public static void SetResolution(int _index)
    {
        Instance.Resolution = Instance.Resolutions[_index];
        Screen.SetResolution(Instance.Resolution.width, Instance.Resolution.height, Instance.IsFullScreen);
    }
    public static void SetFullScreen(bool _isFullscreen)
    {
        Instance.IsFullScreen = _isFullscreen;
        Screen.SetResolution(Instance.Resolution.width, Instance.Resolution.height, Instance.IsFullScreen);
    }
    public static void SetBGMVolume(float _volume)
    {
        AudioManager.SetBGMVolume(_volume);
    }
    public static void SetSEVolume(float _volume)
    {
        AudioManager.SetSEVolume(_volume);
        AudioManager.PlaySE(SEEnum.Pop);
    }
    public static void SetVoiceVolume(float _volume)
    {
        AudioManager.SetVoiceVolume(_volume); ;
        AudioManager.PlayVoice();
    }
    private static void SetLanguageDropdown()
    {
        Instance.LanguageList.Clear();
        Instance.LanguageList.Add("繁體中文");
        Instance.LanguageList.Add("English");
        Instance.LanguageList.Add("日本語");
        Instance.LanguageDropdown.ClearOptions();
        Instance.LanguageDropdown.AddOptions(Instance.LanguageList);
        Instance.LanguageDropdown.value = (int)Instance.Language;
        Instance.LanguageDropdown.RefreshShownValue();
    }
    private static void SetResolutionDropdown()
    {
        Instance.Resolutions = new List<Resolution>();
        List<Resolution> _resolutions = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height}).Distinct().ToList();
        Instance.ResolutionsDropdown.ClearOptions();
        List<string> options = new List<string>();
        for (int i = 0; i < _resolutions.Count ; i++) 
        {
            if (Mathf.Approximately(9f / 16f, (float)_resolutions[i].height / (float)_resolutions[i].width))
            {
                string option = _resolutions[i].width + " x " + _resolutions[i].height;
                options.Add(option);
                Instance.Resolutions.Add(_resolutions[i]);
            }
        }
        Instance.ResolutionsDropdown.AddOptions(options);
        for (int i = 0; i < Instance.Resolutions.Count ;i++)
        {
            if (Instance.Resolutions[i].width == Instance.Resolution.width &&
                Instance.Resolutions[i].height == Instance.Resolution.height)
            {
                Instance.ResolutionsDropdown.value = i;
                break;
            }
            else
            {
                Instance.ResolutionsDropdown.value = Instance.Resolutions.Count - 2;
            }
        }
        Instance.ResolutionsDropdown.RefreshShownValue();
    }
    private static void SetAudioVolumeSlider()
    {
        Instance.BGMVolumeSlider.value = AudioManager.GetBGMVolume();
        Instance.SEVolumeSlider.value = AudioManager.GetSEVolume();
        Instance.VoiceVolumeSlider.value = AudioManager.GetVoiceVolume();
    }
}
public enum LanguageEnum
{
    ZH, EN, JP
}
