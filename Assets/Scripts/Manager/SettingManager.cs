using Assets.Scripts;
using Assets.Scripts.Manager;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour , ISaveManager
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
    public int ResolutionWidth { get; private set; }
    public int ResolutionHeight { get; private set; }
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
    private float TestTime;
    private float DelayTime = 0.2f;
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
        SaveManager.LoadGame();
    }
    private void Update()
    {
        TestTime -= Time.deltaTime;
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
        Instance.ResolutionWidth = Instance.Resolution.width;
        Instance.ResolutionHeight = Instance.Resolution.height;
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
    public static void SetLanguage(int _index)
    {
        Instance.Language = (LanguageEnum)_index;;
        Instance.StartCoroutine(Instance.SetLocale(_index));
        Debug.Log(Instance.Language.ToString());
    }
    public static void SetResolution(int _index)
    {
        Instance.Resolution = Instance.Resolutions[_index];
        Screen.SetResolution(Instance.Resolution.width, Instance.Resolution.height, Instance.IsFullScreen);
        Instance.ResolutionWidth = Instance.Resolution.width;
        Instance.ResolutionHeight = Instance.Resolution.height;
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
        Instance.TestTime = Instance.DelayTime;
        Instance.StartCoroutine(Instance.TestPlaySE());
    }
    public static void SetVoiceVolume(float _volume)
    {
        AudioManager.SetVoiceVolume(_volume);
        Instance.TestTime = Instance.DelayTime;
        Instance.StartCoroutine(Instance.TestPlayVoice());
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
        SetLanguage(Instance.LanguageDropdown.value);
    }
    private static void SetResolutionDropdown()
    {
        Instance.Resolutions = new List<Resolution>();
        List<Resolution> _resolutions = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }).Distinct().ToList();
        Instance.ResolutionsDropdown.ClearOptions();
        List<string> options = new List<string>();
        for (int i = 0; i < _resolutions.Count; i++)
        {
            if (Mathf.Approximately(9f / 16f, (float)_resolutions[i].height / (float)_resolutions[i].width))
            {
                string option = _resolutions[i].width + " x " + _resolutions[i].height;
                options.Add(option);
                Instance.Resolutions.Add(_resolutions[i]);
            }
        }
        Instance.ResolutionsDropdown.AddOptions(options);
        for (int i = 0; i < Instance.Resolutions.Count; i++)
        {
            if (Instance.Resolutions[i].width == Instance.ResolutionWidth &&
                Instance.Resolutions[i].height == Instance.ResolutionHeight)
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
        SetResolution(Instance.ResolutionsDropdown.value);
    }
    private static void SetAudioVolumeSlider()
    {
        Instance.BGMVolumeSlider.value = AudioManager.GetBGMVolume();
        Instance.SEVolumeSlider.value = AudioManager.GetSEVolume();
        Instance.VoiceVolumeSlider.value = AudioManager.GetVoiceVolume();
        Instance.TestTime = 1000f; //避免初始化時 觸發 TestPlay
    }
    private IEnumerator TestPlayVoice()
    {
        yield return new WaitForSeconds(Instance.DelayTime);
        if (Instance.TestTime <= 0)
        {
            AudioManager.PlayVoice(VoiceEnum.Hurt);
            Instance.TestTime = 0f;
        }
    }
    private IEnumerator TestPlaySE()
    {
        yield return new WaitForSeconds(Instance.DelayTime);
        if (Instance.TestTime <= 0)
        {
            AudioManager.PlaySE(SEEnum.Pop);
            Instance.TestTime = 0f;
        }
    }
    private IEnumerator SetLocale(int _localeID)
    {
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_localeID];
    }

    public void LoadData(GameData _data)
    {
        if (_data.PlayerSetting != null)
        {
            SetBGMVolume(_data.PlayerSetting.BGMVolume);
            SetSEVolume(_data.PlayerSetting.SEVolume);
            SetVoiceVolume(_data.PlayerSetting.VoiceVolume);
            Instance.Language = (LanguageEnum)_data.PlayerSetting.LanguageIndex;
            Instance.IsFullScreen = _data.PlayerSetting.IsFullScreen;
            Instance.ResolutionWidth = _data.PlayerSetting.ResolutionWidth;
            Instance.ResolutionHeight = _data.PlayerSetting.ResolutionHeight;
        }
#if !UNITY_WEBPLAYER
        SetResolutionDropdown();
        FullScreenToggle.isOn = IsFullScreen;
#endif
        SetLanguageDropdown();
        SetAudioVolumeSlider();
    }

    public void SaveData(ref GameData _data)
    {
        _data.PlayerSetting.BGMVolume = AudioManager.GetBGMVolume();
        _data.PlayerSetting.SEVolume = AudioManager.GetSEVolume();
        _data.PlayerSetting.VoiceVolume = AudioManager.GetVoiceVolume();
        _data.PlayerSetting.LanguageIndex = (int)Instance.Language;
        _data.PlayerSetting.IsFullScreen = Instance.IsFullScreen;
        _data.PlayerSetting.ResolutionWidth = Instance.ResolutionWidth;
        _data.PlayerSetting.ResolutionHeight = Instance.ResolutionHeight;
    }
}
public enum LanguageEnum
{
    ZH, EN, JP
}
