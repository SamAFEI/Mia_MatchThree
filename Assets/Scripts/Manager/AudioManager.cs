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
    public AudioClip MainBGMClip;
    public AudioClip BattleBGMClip;
    public AudioClip PopFXClip;
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
        Instance.AudioMixer.GetFloat("BGMVolume",out _volume);
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
        if (Instance.BGMSource.isPlaying) return;
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
    public static void PlayPopFX()
    {
        Instance.SESource.clip = Instance.PopFXClip;
        Instance.SESource.Play();
    }
    public static void PlayVoice()
    {
        Instance.VoiceSource.clip = Instance.VoiceClip;
        Instance.VoiceSource.Play();
    }
}
