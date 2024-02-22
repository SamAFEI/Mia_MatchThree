using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool IsPaused { get; private set; }
    public static string CurrentStageBtnName;
    public static string LoadSceneName;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public void PausedGame(bool paused)
    {
        IsPaused = paused;
        if (IsPaused)
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
    public void ExitGame()
    {
        Application.Quit();
    }
}
