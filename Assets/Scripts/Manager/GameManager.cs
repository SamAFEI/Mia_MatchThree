using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
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
    public string CurrentStageBtnName;
    public Stage CurrentStage { get; private set; }
    public static string LoadSceneName;
    public Texture2D CursorDefault;
    public Texture2D CursorSwap;
    public Texture2D CursorBreak;
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
    public static void RegisterCurrentStage(Stage _stage)
    {
        Instance.CurrentStage = _stage;
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
        Application.Quit();
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
        SetCursorDefault();
    }
}
