using Assets.Scripts;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tile : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler
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
                icon.sprite = _item.sprite;
            }
            else
            {
                icon.sprite = null;
            }
        }
    }
    public Image icon;
    public RectTransform rectTransform;
    public Button button;

    public Tile Left => x > 0 ? Board.Instance.Tiles[x - 1, y] : null;
    public Tile Top => y > 0 ? Board.Instance.Tiles[x, y - 1] : null;
    public Tile Right => x < Board.Instance.Width - 1 ? Board.Instance.Tiles[x + 1, y] : null;
    public Tile Bottom => y < Board.Instance.Height - 1 ? Board.Instance.Tiles[x, y + 1] : null;
    public Tile[] Neighbours => new[] { Left, Right, Top, Bottom };
    private void Awake()
    {
        icon = transform.Find("Image").GetComponent<Image>();
        rectTransform = icon.GetComponent<RectTransform>();
        button = GetComponent<Button>();
    }

    private void Start()
    {
        //button.onClick.AddListener(() => Board.Instance.SelectTile(this));
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
        if (Left != null && Right != null && Left.Item.color == Item.color && Right.Item.color == Item.color)
        {
            result.Add(Left);
            result.AddRange(Right.GetConnectedTiles(exclude));
        }
        if (Top != null && Bottom != null && Top.Item.color == Item.color && Bottom.Item.color == Item.color)
        {
            result.Add(Top);
            result.AddRange(Bottom.GetConnectedTiles(exclude));
        }

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
    public void SetIconAlpha(bool value)
    {
        if (value)
        {
            icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, 255);
        }
        else
        {
            icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, 0);
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
    }

    public void OnPointerDown(PointerEventData eventData)
    {
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
}
