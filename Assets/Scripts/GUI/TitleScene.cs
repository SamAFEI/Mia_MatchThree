using Assets.Scripts.Manager;
using UnityEngine;

public class TitleScene : MonoBehaviour
{
    private void Start()
    {
        AudioManager.PlayMainBGM();
        GameManager.SetCursorDefault();
    }
    private void Update()
    {
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                SaveManager.Instance.DeleteSaveData();
            }
            else
            {
                GameManager.Instance.LoadMainScene();
            }
        }
    }
}
