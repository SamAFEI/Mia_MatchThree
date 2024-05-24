using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<AudioManager>();
            }
            if (instance == null)
            {
                CreateDefault();
            }
            return instance;
        }
    }
    public AudioMixer AudioMixer { get; private set; }
    public AudioSource BGMSource { get; private set; }
    public AudioSource SESource { get; private set; }
    public AudioSource VoiceSource { get; private set; }
    public AudioSource SexSource { get; private set; }
    public AudioSource SexVoiceSource { get; private set; }
    public AudioClip MainBGMClip;
    public AudioClip BattleBGMClip;
    [Header("消除符文")]
    public AudioClip PopFXClip;
    [Header("技能升級")]
    public AudioClip LevelUpClip;
    [Header("失敗")]
    public AudioClip FailedClip;
    [Header("勝利")]
    public AudioClip VictoryClip;
    [Header("魔法擊中")]
    public AudioClip ImapctClip;
    [Header("回復")]
    public AudioClip HealClip;
    [Header("按鈕")]
    public AudioClip ClickClip;
    [Header("刀痕")]
    public AudioClip SlashClip;
    [Header("中毒")]
    public AudioClip PoisonClip;
    [Header("Debuff")]
    public AudioClip DebuffClip;

    [Header("語音")]
    public AudioClip VoiceClip;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        //Instance = this;
        DontDestroyOnLoad(this);
    }
    private static void CreateDefault()
    {
        GameObject obj = Resources.Load<GameObject>("Prefabs/Manager/AudioManager");
        obj = Instantiate(obj, Vector3.zero, Quaternion.identity);
        instance = obj.GetComponent<AudioManager>();
        Instance.AudioMixer = Resources.Load<AudioMixer>("Audio/MainMixer");
        Instance.BGMSource = obj.transform.Find("BGM").GetComponent<AudioSource>();
        Instance.BGMSource.loop = true;
        Instance.SESource = obj.transform.Find("SE").GetComponent<AudioSource>();
        Instance.SESource.loop = false;
        Instance.VoiceSource = obj.transform.Find("Voice").GetComponent<AudioSource>();
        Instance.VoiceSource.loop = false;
        Instance.SexVoiceSource = obj.transform.Find("SexVoice").GetComponent<AudioSource>();
        Instance.SexVoiceSource.loop = true;
        Instance.SexSource = obj.transform.Find("Sex").GetComponent<AudioSource>();
        Instance.SexVoiceSource.loop = true;
    }
    public static void SetBGMVolume(float _volume)
    {
        Instance.AudioMixer.SetFloat("BGMVolume", _volume);
    }
    public static void SetSEVolume(float _volume)
    {
        Instance.AudioMixer.SetFloat("SEVolume", _volume);
    }
    public static void SetVoiceVolume(float _volume)
    {
        Instance.AudioMixer.SetFloat("VoiceVolume", _volume);
    }
    public static float GetBGMVolume()
    {
        float _volume;
        Instance.AudioMixer.GetFloat("BGMVolume", out _volume);
        return _volume;
    }
    public static float GetSEVolume()
    {
        float _volume;
        Instance.AudioMixer.GetFloat("SEVolume", out _volume);
        return _volume;
    }
    public static float GetVoiceVolume()
    {
        float _volume;
        Instance.AudioMixer.GetFloat("VoiceVolume", out _volume);
        return _volume;
    }
    public static void PlayMainBGM()
    {
        if (Instance.BGMSource.clip == Instance.MainBGMClip && Instance.BGMSource.isPlaying) return;
        Instance.BGMSource.clip = Instance.MainBGMClip;
        Instance.BGMSource.Play();
    }
    public static void StopBGM()
    {
        Instance.BGMSource.Stop();
    }
    public static void PlayBattleBGM()
    {
        Instance.BGMSource.clip = Instance.BattleBGMClip;
        Instance.BGMSource.Play();
    }
    public static void PlayVoice(VoiceEnum type)
    {
        Instance.VoiceSource.loop = false;
        if (type == VoiceEnum.Hurt) { Instance.VoiceSource.clip = GameManager.Instance.CurrentStage.Data.HurtClip; }
        else if (type == VoiceEnum.Failed) {  }
        else if (type == VoiceEnum.Victory) { }
        else if (type == VoiceEnum.Attack) { Instance.VoiceSource.clip = GameManager.Instance.CurrentStage.Data.AttackClip; }
        Instance.VoiceSource.PlayOneShot(Instance.VoiceSource.clip, 1f);
    }
    public void PlaySex(int _index)
    {
        Instance.SexSource.Stop();
        Instance.SexSource.clip = GameManager.Instance.CurrentStage.Data.SexClips[_index];
        Instance.SexSource.Play();
    }
    public void StopSexSource()
    {
        Instance.SexSource.Stop();
    }
    public void PlaySexVoice(int _index)
    {
        Instance.SexVoiceSource.Stop();
        Instance.SexVoiceSource.clip = GameManager.Instance.CurrentStage.Data.SexVoiceClips[_index];
        Instance.SexVoiceSource.Play();
    }
    public void StopSexVoiceSource()
    {
        Instance.SexVoiceSource.Stop();
    }
    public static void PlaySE(SEEnum type)
    {
        if (type == SEEnum.Pop) { Instance.SESource.clip = Instance.PopFXClip; }
        else if (type == SEEnum.LevelUp) { Instance.SESource.clip = Instance.LevelUpClip; }
        else if (type == SEEnum.Victory) { Instance.SESource.clip = Instance.VictoryClip; }
        else if (type == SEEnum.Failed) { Instance.SESource.clip = Instance.FailedClip; }
        else if (type == SEEnum.Imapct) { Instance.SESource.clip = Instance.ImapctClip; }
        else if (type == SEEnum.Heal) { Instance.SESource.clip = Instance.HealClip; }
        else if (type == SEEnum.Click) { Instance.SESource.clip = Instance.ClickClip; }
        else if (type == SEEnum.Slash) { Instance.SESource.clip = Instance.SlashClip; }
        else if (type == SEEnum.Debuff) { Instance.SESource.clip = Instance.DebuffClip; }
        else if (type == SEEnum.Poison) { Instance.SESource.clip = Instance.PoisonClip; }
        Instance.SESource.PlayOneShot(Instance.SESource.clip, 0.7f);
        //Instance.SESource.Play();
    }
    public void PlayClick()
    {
        PlaySE(SEEnum.Click);
    }
}
public enum SEEnum
{
    Pop, LevelUp, Failed, Victory, Imapct, Heal, Click, Slash, Debuff, Poison
}
public enum VoiceEnum
{
    Hurt, Failed, Victory, Attack
}
