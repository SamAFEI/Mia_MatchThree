using Assets.Scripts;
using Assets.Scripts.Manager;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveManager
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<GameManager>();
            }
            if (instance == null)
            {
                CreateDefault();
            }
            return instance;
        }
    }
    public bool IsPaused { get; private set; }
    public Stage CurrentStage { get; private set; }
    public List<Stage> Stages { get; private set; } = new List<Stage>();
    public static string LoadSceneName;
    public Texture2D CursorDefault;
    public Texture2D CursorSwap;
    public Texture2D CursorBreak;
    public bool IsMosaic;
    public bool IsHelped;
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
    private void Start()
    {
        SaveManager.LoadGame();
        SettingManager.SetAreaActive(false);
    }
    public static void RegisterCurrentStage(Stage _stage)
    {
        Instance.CurrentStage = _stage;
    }
    public static void CompleteStage()
    {
        Instance.CurrentStage.Data.IsComplete = true;
    }
    public static void PausedGame(bool paused)
    {
        Instance.IsPaused = paused;
        if (Instance.IsPaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
    public void LoadBattleScene()
    {
        LoadSceneName = "MatchThreeScene";
        SceneManager.LoadScene("LoadingScene");
    }
    public void LoadMainScene()
    {
        LoadSceneName = "MainScene";
        SceneManager.LoadScene("LoadingScene");
    }
    public void LoadTalkScene()
    {
        LoadSceneName = "TalkScene";
        SceneManager.LoadScene("LoadingScene");
    }
    public void LoadCGScene()
    {
        LoadSceneName = "CGScene";
        SceneManager.LoadScene("LoadingScene");
    }
    public void LoadTitleScene()
    {
        LoadSceneName = "TitleScene";
        SceneManager.LoadScene("LoadingScene");
    }
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
        //Application.OpenURL(webplayerQuitURL);
#else
        Application.Quit();
#endif
    }
    public static void SetCursorSwap()
    {
        if (!SkillManager.Instance.IsBreakSkill)
        {
            Cursor.SetCursor(Instance.CursorSwap, new Vector2(21, 21), CursorMode.ForceSoftware);
        }
        else
        {
            SetCursorBreak();
        }
    }
    public static void SetCursorDefault()
    {
        Cursor.SetCursor(Instance.CursorDefault, new Vector2(25, 17), CursorMode.ForceSoftware);
    }
    public static void SetCursorBreak()
    {
        Cursor.SetCursor(Instance.CursorBreak, new Vector2(23, 21), CursorMode.ForceSoftware);
    }
    private static void CreateDefault()
    {
        GameObject obj = Resources.Load<GameObject>("Prefabs/Manager/GameManager");
        obj = Instantiate(obj, Vector3.zero, Quaternion.identity);
        instance = obj.GetComponent<GameManager>();
        Instance.CursorDefault = Resources.Load<Texture2D>("Sprites/GUI/CursorDefault");
        Instance.CursorSwap = Resources.Load<Texture2D>("Sprites/GUI/CursorSwap");
        Instance.CursorBreak = Resources.Load<Texture2D>("Sprites/GUI/CursorBreak");
        InitStages();
        Stage stage = Resources.Load<Stage>("Prefabs/Stage/Stage01");
        Instance.CurrentStage = stage;
        SetCursorDefault();
    }
    private static void InitStages()
    {
        Instance.Stages.AddRange(Resources.LoadAll<Stage>("Prefabs/Stage/").ToList<Stage>());
        foreach (Stage stage in Instance.Stages)
        {
            if (stage.Data != null) { stage.Data.IsComplete = false; }
        }
    }

    public static IEnumerator ShakeCamera(float _duration, float magnitude)
    {
        Camera _camera = Camera.main;
        Vector3 orignalPos = _camera.transform.localPosition;
        float elapsed = 0.0f;
        while (elapsed < _duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            _camera.transform.localPosition = new Vector3(x, y, orignalPos.z);
            elapsed += Time.deltaTime;
            yield return null;
        }
        _camera.transform.localPosition = orignalPos;
        yield return null;
    }

    public void LoadData(GameData _data)
    {
        Instance.IsHelped = _data.IsHelped;
        if (_data.CurrentStage != null)
        {
            foreach (Stage stage in Instance.Stages)
            {
                if (stage.Data != null && stage.Data.StageName == _data.CurrentStage.StageName)
                {
                    Instance.CurrentStage = stage;
                    break;
                }
            }
        }
        if (_data.Stages != null)
        {
            foreach (Stage stage in Instance.Stages)
            {
                StageStore _store = _data.Stages.Where(x => x.StageName == stage.StageName).FirstOrDefault();
                if (_store != null)
                {
                    stage.StageName = _store.StageName;
                    stage.Data.IsComplete = _store.IsComplete;
                }
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.IsHelped = Instance.IsHelped;
        _data.CurrentStage = new StageStore(Instance.CurrentStage.Data);
        _data.Stages = new List<StageStore>();
        foreach (Stage stage in Instance.Stages)
        {
            if (stage.Data == null) { continue; }
            _data.Stages.Add(new StageStore(stage.Data));
        }
    }
}
