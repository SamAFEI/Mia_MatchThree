using UnityEngine;

public class TitleScene : MonoBehaviour
{
    private void Start()
    {
        AudioManager.PlayMainBGM();
    }
    private void Update()
    {
        if (Input.anyKeyDown)
        {
            GameManager.Instance.LoadMainScene();
        }
    }
}
