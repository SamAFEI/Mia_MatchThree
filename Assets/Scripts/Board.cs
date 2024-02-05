using Assets.Scripts;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    public static Board Instance { get; private set; }

    public TextMeshProUGUI Text;
    public ScrollRect scrollRect;
    public Row[] rows;
    public Tile[,] Tiles { get; private set; }
    public bool[,] CanPopTiles { get; private set; } 
    public bool IsBusy { get; private set; }
    public int Width => Tiles.GetLength(0);
    public int Height => Tiles.GetLength(1);

    public Tile DragTile;

    private List<Tile> _selection = new List<Tile>();
    private List<Tile> _fallTiles = new List<Tile>();
    private List<Tile> _newTopTiles = new List<Tile>();
    private List<Tile> _bonusTiles = new List<Tile>();
    private List<Tile> _moveTiles = new List<Tile>();
    private List<(Item, int)> _hitList = new List<(Item, int)>();
    private const float TweenDuration = 0.25f;
    private const float FallDuration = 0.05f;
    private void Awake()
    {
        Instance = this;
        DragTile = null;
    }

    private void Start()
    {
        IsBusy = true;
        Tiles = new Tile[rows.Max(row => row.tiles.Length), rows.Length];

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                Tile tile = rows[y].tiles[x];

                tile.x = x;
                tile.y = y;

                tile.Item = ItemDatabase.Items[Random.Range(0, ItemDatabase.Items.Length)];
                tile.Item.level = 0;
                Tiles[x, y] = tile;
                tile.transform.name = "[" + tile.x + ", " + tile.y + "]";
            }
        }
        CanPop(true);
        if (CanPop())
        {
            Pop();
        }
        IsBusy = false;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            foreach (Tile tile in Tiles[0, 0].GetConnectedTiles())
            {
                tile.icon.transform.DOScale(1.25f, TweenDuration).Play();
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Time.timeScale = 1;
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Tile tile = rows[y].tiles[x];
                    tile.transform.name = "[" + tile.x + ", " + tile.y + "]" + " LV"+tile.Item.level;
                }
            }
        }
    }

    #region 交換
    public async void ChangeTile(Tile originTile, Tile targetTile)
    {
        if (IsBusy) { return; }
        IsBusy = true;
        _moveTiles.Clear();
        _moveTiles.Add(originTile);
        _moveTiles.Add(targetTile);
        await Swap(originTile, targetTile);

        if (targetTile.Item != null && CanPop())
        {
            await Pop();
        }
        else
        {
            await Swap(targetTile, originTile);
        }
        DragTile = null;
        IsBusy = false;
    }
    public async void SelectTile(Tile tile)
    {
        if (_selection.Count == 1 && !_selection[0].DoCheckNeighbour(tile))
        {
            return;
        }
        if (!_selection.Contains(tile))
        {
            _selection.Add(tile);
        }

        if (_selection.Count < 2) { return; }

        await Swap(_selection[0], _selection[1]);

        if (CanPop())
        {
            await Pop();
        }
        else
        {
            await Swap(_selection[0], _selection[1]);
        }

        _selection.Clear();
    }

    public async Task Swap(Tile tile1, Tile tile2)
    {
        Image icon1 = tile1.icon;
        Image icon2 = tile2.icon;

        Transform transform1 = icon1.transform;
        Transform transform2 = icon2.transform;

        Sequence sequence = DOTween.Sequence();

        sequence.Join(transform1.DOMove(transform2.position, TweenDuration))
                .Join(transform2.DOMove(transform1.position, TweenDuration));

        await sequence.Play()
                    .AsyncWaitForCompletion();

        transform1.SetParent(tile2.transform);
        transform2.SetParent(tile1.transform);

        tile1.icon = icon2;
        tile2.icon = icon1;

        Item tileItem = tile1.Item;
        tile1.Item = tile2.Item;
        tile2.Item = tileItem;
    }
    #endregion

    #region 消除
    private bool CanPop(bool _isChange = false)
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                if (Tiles[x, y].GetConnectedTiles().Skip(1).Count() >= 2)
                {
                    if (_isChange) //開始檢查用 避免連線
                    {
                        while ((Tiles[x, y].GetConnectedTiles().Skip(1).Count() >= 2))
                        {//有連線就換Tile
                            Tiles[x, y].Item = ItemDatabase.Items[Random.Range(0, ItemDatabase.Items.Length)];
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
    private async Task Pop()
    {
        List<Tile> connectedTiles = new List<Tile>();
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                Tile tile = Tiles[x, y];
                List<Tile> tiles = tile.GetConnectedTiles();
                
                //排除已連線Tile
                if (connectedTiles.IndexOf(tile) >= 0) continue;
                if (tiles.Skip(1).Count() < 2) continue;
                //Bonus的消除
                foreach (Tile _tile in tiles)
                {
                    if (_tile.Item.level == 1)
                    {
                        if (_tile.Item.isEffectH)
                        {
                            for (int i = 0; i < Width; i++)
                            {
                                connectedTiles.Add(Tiles[i, _tile.y]);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < Height; i++)
                            {
                                connectedTiles.Add(Tiles[_tile.x, i]);
                            }
                        }
                    }
                    else if (_tile.Item.level == 2)
                    {
                        for (int i = 0; i < Width; i++)
                        {
                            connectedTiles.Add(Tiles[i, _tile.y]);
                        }
                        for (int i = 0; i < Height; i++)
                        {
                            connectedTiles.Add(Tiles[_tile.x, i]);
                        }
                    }
                }
                //一般的消除
                if (tiles.Count() > 3) //3消以上 產生BounsTile
                {
                    //讓有移動的Tile 優先產生BonusTile
                    foreach (Tile _tile in tiles)
                    {   
                        if (_moveTiles.IndexOf(_tile) > -1)
                        {
                            tile = _tile;
                            break;
                        }
                    }
                    if (tiles.Count() == 4)
                        tile.Item.bonusLevel = 1;
                    if (tiles.Count() > 4)
                        tile.Item.bonusLevel = 2;
                    _bonusTiles.Add(tile);
                }
                _hitList.Add( (tile.Item, tiles.Count()) );
                connectedTiles.AddRange(tiles);
                connectedTiles = connectedTiles.Distinct(new TileCompare()).ToList();
                _moveTiles.Clear();
            }
        }
        //變大1.5倍
        Sequence Sequence = DOTween.Sequence();
        foreach (Tile connectedTile in connectedTiles)
        {
            Sequence.Join(connectedTile.icon.transform.DOScale(Vector3.one * 1.5f, TweenDuration));
        }
        await Sequence.Play().AsyncWaitForCompletion();

        //縮到最小 消失效果
        Sequence deflateSequence = DOTween.Sequence();
        foreach (Tile connectedTile in connectedTiles)
        {
            deflateSequence.Join(connectedTile.icon.transform.DOScale(Vector3.zero, FallDuration));
        }
        await deflateSequence.Play()
                            .AsyncWaitForCompletion();

        foreach (Tile tile in _bonusTiles)
        {
            connectedTiles.Remove(tile);
        }
        Calculate();
        await NewBounsTile();

        //清空圖片 設定透明 還原大小
        Sequence inflateSequence = DOTween.Sequence();
        foreach (Tile connectedTile in connectedTiles)
        {
            connectedTile.Item = null;
            connectedTile.SetIconAlpha(false);
            inflateSequence.Join(connectedTile.icon.transform.DOScale(Vector3.one, TweenDuration));
        }
        await inflateSequence.Play()
                            .AsyncWaitForCompletion();

        await FallTile();
        if (CanPop())
        {
            await Pop();
        }
    }
    #endregion

    #region 掉落
    private async Task FallTile()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = (Height - 1); y >= 0; y--)
            {
                //最上面沒Tile 表示要產生新的Tile
                if (Tiles[x, y].Top == null && Tiles[x, y].Item == null)
                {
                    _newTopTiles.Add(Tiles[x, y]);
                    continue;
                }
                //自己有Item or Top沒有Item 不動作
                if (Tiles[x, y].Item != null || Tiles[x, y].Top.Item == null) 
                { continue; }

                //自己沒有 Item && Top有Item 可以掉落
                _fallTiles.Add(Tiles[x, y - 1]); // 紀錄那些需要往下掉
            }
        }
        await DoFill();
    }

    private async Task DoFill()
    {
        await NewTopTile();
        if (_fallTiles.Count == 0) { return; }
        List<Tile> Tiles = new List<Tile>();
        _moveTiles.AddRange(_fallTiles); //紀錄移動的Tile
        Tiles.AddRange(_fallTiles);
        Sequence sequence = DOTween.Sequence();
        foreach (Tile _tile in Tiles)
        {
            Tile tile1 = _tile;
            Tile tile2 = _tile.Bottom;
            Image icon1 = tile1.icon;
            Image icon2 = tile2.icon;

            Transform transform1 = icon1.transform;
            Transform transform2 = icon2.transform;

            sequence.Join(transform1.DOMove(transform2.position, FallDuration))
                    .Join(transform2.DOMove(transform1.position, FallDuration));
            //Debug.Log(_tile.name);

            transform1.SetParent(tile2.transform);
            transform2.SetParent(tile1.transform);

            tile1.icon = icon2;
            tile2.icon = icon1;

            Item tileItem = tile1.Item;
            tile1.Item = tile2.Item;
            tile2.Item = tileItem;
            _fallTiles.Remove(_tile);
        }
        await sequence.Play()
            .AsyncWaitForCompletion();
        await FallTile();
    }
    #endregion

    #region 產生新的
    private async Task NewTopTile()
    {
        if (_newTopTiles.Count == 0) { return; }
        List<Tile> Tiles = new List<Tile>();
        Tiles.AddRange(_newTopTiles);
        Sequence sequence = DOTween.Sequence();

        foreach (Tile _tile in Tiles)
        {
            _tile.Item = ItemDatabase.Items[Random.Range(0, ItemDatabase.Items.Length)];
            _tile.Item.bonusLevel = 0; // Bonus產出的會回寫到物件上 要改回0
            sequence.Join(_tile.icon.transform.DOScale(Vector3.one, 0));
            _tile.SetIconAlpha(true);
            if (_tile.Bottom.Item == null) //下面是空的時要掉落
            {
                _fallTiles.Add(_tile);
            }
            _newTopTiles.Remove(_tile);
        }
        await sequence.Play()
            .AsyncWaitForCompletion();
    }

    private async Task NewBounsTile()
    {
        if (_bonusTiles.Count == 0) { return; }
        List<Tile> Tiles = new List<Tile>();
        Tiles.AddRange(_bonusTiles);
        Sequence sequence = DOTween.Sequence();

        foreach (Tile _tile in Tiles)
        {
            if (_tile.Item.color == ItemColorEnum.Red)
            {
                if (_tile.Item.bonusLevel == 2)
                    _tile.Item = ItemDatabase.RedPlus;
                else if (_tile.Item.bonusLevel == 1)
                    _tile.Item = ItemDatabase.RedBonus[Random.Range(0, ItemDatabase.RedBonus.Length)];
            }
            else if (_tile.Item.color == ItemColorEnum.Blue)
            {
                if (_tile.Item.bonusLevel == 2)
                    _tile.Item = ItemDatabase.BluePlus;
                else if (_tile.Item.bonusLevel == 1)
                    _tile.Item = ItemDatabase.BlueBonus[Random.Range(0, ItemDatabase.BlueBonus.Length)];
            }
            else if (_tile.Item.color == ItemColorEnum.Green)
            {
                if (_tile.Item.bonusLevel == 2)
                    _tile.Item = ItemDatabase.GreenPlus;
                else if (_tile.Item.bonusLevel == 1)
                    _tile.Item = ItemDatabase.GreenBonus[Random.Range(0, ItemDatabase.GreenBonus.Length)];
            }
            else if (_tile.Item.color == ItemColorEnum.Yellow)
            {
                if (_tile.Item.bonusLevel == 2)
                    _tile.Item = ItemDatabase.YellowPlus;
                else if (_tile.Item.bonusLevel == 1)
                    _tile.Item = ItemDatabase.YellowBonus[Random.Range(0, ItemDatabase.YellowBonus.Length)];
            }
            sequence.Join(_tile.icon.transform.DOScale(Vector3.one, TweenDuration));
            _tile.SetIconAlpha(true); 
            _bonusTiles.Remove(_tile);
        }
        await sequence.Play()
            .AsyncWaitForCompletion();
    }
    #endregion

    #region 計算
    private void Calculate()
    {
        List<(Item, int)> list = new List<(Item, int)>();
        list.AddRange(_hitList);
        foreach ((Item, int) _hit in list)
        {
            Text.text += _hit.Item1.color.ToString() + " = " + _hit.Item2 + "\n";
            scrollRect.normalizedPosition = Vector2.zero; //Scroll to Bottom
            _hitList.Remove(_hit);
        }
    }
    #endregion
}
