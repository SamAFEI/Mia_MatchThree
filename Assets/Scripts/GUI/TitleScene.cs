using Assets.Scripts.Manager;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TitleScene : MonoBehaviour
{
    public Image Title;
    private void Start()
    {
        AudioManager.PlayMainBGM();
        GameManager.SetCursorDefault();
        InvokeRepeating("StartFade", 0f, 2f);
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
    private void StartFade()
    {
        StartCoroutine(FadeTitle());
    }
    private IEnumerator FadeTitle()
    {
        for (float alpha = 0.3f; alpha < 1f; alpha += Time.deltaTime)
        {
            Title.color = new Color(Title.color.r, Title.color.g, Title.color.b, alpha);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        for (float alpha = 1f; alpha > 0.3f; alpha -= Time.deltaTime)
        {
            Title.color = new Color(Title.color.r, Title.color.g, Title.color.b, alpha);
            yield return null;
        }
    }
}
