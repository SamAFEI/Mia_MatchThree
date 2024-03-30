using Assets.Scripts;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tile : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public int x;
    public int y;
    public string Index { get => x + "," + y; }
    private Item _item;
    public Item Item
    {
        get => _item;
        set
        {
            if (_item == value) return;
            _item = value;
            if (_item != null)
            {
                rune.sprite = _item.rune;
                frame.sprite = _item.frame;
                debuffIndex = _item.debuffIndex;
                color = _item.color;
            }
            else
            {
                rune.sprite = null;
                frame.sprite = null;
                debuffIndex = ItemDebuffEnum.Non;
            }
        }
    }
    public Image rune;
    public Image frame;
    public Image debuffIcon;
    public Button button;
    public ItemColorEnum color;
    public ItemDebuffEnum debuffIndex = ItemDebuffEnum.Non;
    private List<Sprite> _debuffIcons = new List<Sprite>();

    public Tile Left => x > 0 ? Board.Instance.Tiles[x - 1, y] : null;
    public Tile Top => y > 0 ? Board.Instance.Tiles[x, y - 1] : null;
    public Tile Right => x < Board.Instance.Width - 1 ? Board.Instance.Tiles[x + 1, y] : null;
    public Tile Bottom => y < Board.Instance.Height - 1 ? Board.Instance.Tiles[x, y + 1] : null;
    public Tile[] Neighbours => new[] { Left, Right, Top, Bottom };
    private void Awake()
    {
        frame = this.transform.Find("frame").GetComponent<Image>();
        rune = frame.transform.Find("rune").GetComponent<Image>();
        debuffIcon = rune.transform.Find("debuffIcon").GetComponent<Image>();
        button = GetComponent<Button>();
    }

    private void Start()
    {
        _debuffIcons.AddRange(SkillManager.Instance.DebuffIcons);
        //button.onClick.AddListener(() => Board.Instance.SelectTile(this));
    }
    private void LateUpdate()
    {
        debuffIcon.sprite = _debuffIcons[(int)debuffIndex];
        if (debuffIndex == ItemDebuffEnum.Non)
        {
            debuffIcon.color = new Color(debuffIcon.color.r, debuffIcon.color.g, debuffIcon.color.b, 0); 
        }
        else
        {
            debuffIcon.color = new Color(debuffIcon.color.r, debuffIcon.color.g, debuffIcon.color.b, 1);
        }
    }
    public List<Tile> GetConnectedTiles(List<Tile> exclude = null)
    {
        List<Tile> result = new List<Tile>() { this, };

        if (exclude == null)
        {
            exclude = new List<Tile>() { this, };
        }
        else
        {
            exclude.Add(this);
        }
        if (this.Item == null) { return result; }

        List<Tile> resultH = new List<Tile>();
        List<Tile> resultV = new List<Tile>();
        //找 水平連線 Tiles 
        resultH.Clear();
        resultH.AddRange(GetConnectedTiles(true));
        if (resultH.Count() > 2)
        {
            result.AddRange(resultH);
            //水平 Tiles 找每個 垂直 Tiles
            foreach (Tile _tile in resultH)
            {
                resultV.Clear();
                resultV.AddRange(_tile.GetConnectedTiles(false));
                if (resultV.Count() > 2)
                {
                    result.AddRange(resultV);
                }
            }
        }
        //找 垂直連線 Tiles 
        resultV.Clear();
        resultV.AddRange(GetConnectedTiles(false));
        if (resultV.Count() > 2)
        {
            result.AddRange(resultV);
            //垂直 Tiles 找每個 水平 Tiles
            foreach (Tile _tile in resultV)
            {
                resultH.Clear();
                resultH.AddRange(_tile.GetConnectedTiles(true));
                if (resultH.Count() > 2)
                {
                    result.AddRange(resultH);
                }
            }
        }
        /*if (Left != null && Right != null && Left.Item.color == Item.color && Right.Item.color == Item.color)
        {
            result.Add(Left);
            result.AddRange(Right.GetConnectedTiles(exclude));
        }
        if (Top != null && Bottom != null && Top.Item.color == Item.color && Bottom.Item.color == Item.color)
        {
            result.Add(Top);
            result.AddRange(Bottom.GetConnectedTiles(exclude));
        }*/

        /*foreach (Tile neighbour in Neighbours)
        {
            if (neighbour == null || exclude.Contains(neighbour) || neighbour.Item != Item)
            {
                continue;
            }
            result.AddRange(neighbour.GetConnectedTiles(exclude));
        }*/
        //List過濾重複 使用 Distinct (方法很多種)
        return result.Distinct(new TileCompare()).ToList();
        //return result.DistinctBy(tile => tile.Index).ToList();
    }
    public List<Tile> GetConnectedTiles(bool IsHorizontal, List<Tile> exclude = null)
    {
        List<Tile> result = new List<Tile>() { this, };

        if (exclude == null)
        {
            exclude = new List<Tile>() { this, };
        }
        else
        {
            exclude.Add(this);
        }
        if (this.Item == null) { return result; }

        if (IsHorizontal)
        {
            if (Left != null && Left.Item.color == Item.color && !exclude.Contains(Left))
            {
                result.AddRange(Left.GetConnectedTiles(IsHorizontal, exclude));
            }
            if (Right != null && Right.Item.color == Item.color && !exclude.Contains(Right))
            {
                result.AddRange(Right.GetConnectedTiles(IsHorizontal, exclude));
            }
        }
        else
        {
            if (Top != null && Top.Item.color == Item.color && !exclude.Contains(Top))
            {
                //result.Add(Top);
                result.AddRange(Top.GetConnectedTiles(IsHorizontal, exclude));
            }
            if (Bottom != null && Bottom.Item.color == Item.color && !exclude.Contains(Bottom))
            {
                result.AddRange(Bottom.GetConnectedTiles(IsHorizontal, exclude));
            }
        }
        //List過濾重複 使用 Distinct (方法很多種)
        return result.Distinct(new TileCompare()).ToList();
        //return result.DistinctBy(tile => tile.Index).ToList();
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
    public bool DoCheckNeighbour(Tile tile)
    {
        if (tile == null) { return false; }
        foreach (Tile neighbour in Neighbours)
        {
            if (neighbour == tile)
            {
                return true;
            }
        }
        return false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!Board.Instance.IsBusy && Board.Instance.DragTile != null
                && Board.Instance.DragTile.DoCheckNeighbour(this))
        {
            Board.Instance.ChangeTile(Board.Instance.DragTile, this);
        }
        GameManager.SetCursorSwap();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (SkillManager.Instance.IsBreakSkill)
        {
            Board.Instance.BreakTile(this);
        }
        if (!Board.Instance.IsBusy)
        {
            Board.Instance.DragTile = this;
            //Debug.Log(Board.Instance.DragTile.Index);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Board.Instance.DragTile = null;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.SetCursorDefault();
    }
}
