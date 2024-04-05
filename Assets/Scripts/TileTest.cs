using UnityEngine;
using UnityEngine.UI;

public class TileTest : MonoBehaviour
{
    public int x;
    public int y;
    public string Index { get => x + "," + y; }
    public Item Item;
    public Image rune;
    public Image frame;
    public Image debuffIcon;
    public Button button;
    public ItemColorEnum color;
    public ItemDebuffEnum debuffIndex = ItemDebuffEnum.Non;
    private void Awake()
    {
        frame = this.transform.Find("frame").GetComponent<Image>();
        rune = frame.transform.Find("rune").GetComponent<Image>();
        debuffIcon = rune.transform.Find("debuffIcon").GetComponent<Image>();
        button = GetComponent<Button>();
    }

    private void Start()
    {
        //button.onClick.AddListener(() => Board.Instance.SelectTile(this));
        if (Item != null)
        {
            rune.sprite = Item.rune;
            frame.sprite = Item.frame;
            color = Item.color;
            SetIconAlpha(true);
        }
    }
    private void LateUpdate()
    {
        if (debuffIndex == ItemDebuffEnum.Non)
        {
            debuffIcon.color = new Color(debuffIcon.color.r, debuffIcon.color.g, debuffIcon.color.b, 0);
        }
        else
        {
            debuffIcon.color = new Color(debuffIcon.color.r, debuffIcon.color.g, debuffIcon.color.b, 0.8f);
        }
    }
    public void SetIconAlpha(bool value)
    {
        if (value)
        {
            rune.color = new Color(rune.color.r, rune.color.g, rune.color.b, 255);
            frame.color = new Color(frame.color.r, frame.color.g, frame.color.b, 255);
        }
        else
        {
            rune.color = new Color(rune.color.r, rune.color.g, rune.color.b, 0);
            frame.color = new Color(frame.color.r, frame.color.g, frame.color.b, 0);
        }
    }

}
