using Assets.Scripts;
using Assets.Scripts.Manager;
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
    public GameObject Result;
    public Row[] rows;
    public Tile[,] Tiles { get; private set; }
    public bool[,] CanPopTiles { get; private set; }
    public bool IsBusy { get; private set; }
    private int currentTrun;
    public int Width => Tiles.GetLength(0);
    public int Height => Tiles.GetLength(1);

    public Tile DragTile;

    private List<Tile> _selection = new List<Tile>();
    private List<Tile> _fallTiles = new List<Tile>();
    private List<Tile> _newTopTiles = new List<Tile>();
    private List<Tile> _bonusTiles = new List<Tile>();
    private List<Tile> _moveTiles = new List<Tile>();
    private List<(Item, int, Vector3)> _hitList = new List<(Item, int, Vector3)>();
    private float TweenDuration = 0.25f;
    private float FallDuration = 0.05f;
    private const int MaxTrun = 40;
    private const float HitBonus = 0.5f;
    private int combos;
    public int MaxCombos;
    public float ATKDown;
    public float DEFDown;
    public float Poison;
    private void Awake()
    {
        Instance = this;
        DragTile = null;
        Result = GameObject.Find("Result");
    }
    private void Start()
    {
        AudioManager.PlayBattleBGM();
        TooltipManager.ShowToolTip("");
        Result.SetActive(false);
        currentTrun = 1;
        IsBusy = true;
        Tiles = new Tile[rows.Max(row => row.tiles.Length), rows.Length];
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                Tile tile = rows[y].tiles[x];

                tile.x = x;
                tile.y = y;

                Item newItem = ItemDatabase.Items[Random.Range(0, ItemDatabase.Items.Count)];
                tile.Item = Instantiate(newItem);
                // Bonus產出的會回寫到物件上 預設0 避免問題
                tile.Item.level = 0;
                tile.Item.bonusLevel = 0;
                tile.debuffIndex = ItemDebuffEnum.Non;
                Tiles[x, y] = tile;
                tile.transform.name = "[" + tile.x + ", " + tile.y + "]";
                tile.frame.transform.name = "[" + tile.x + ", " + tile.y + "]";
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
        if (Input.GetKeyDown(KeyCode.V))
        {
            Result.SetActive(true);
            BattleResult.Instance.GameResult(true);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveManager.SaveGame();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            SaveManager.LoadGame();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            foreach (Tile tile in Tiles[0, 0].GetConnectedTiles())
            {
                tile.rune.transform.DOScale(1.25f, TweenDuration).Play();
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
                    tile.transform.name = "[" + tile.x + ", " + tile.y + "]" + " LV" + tile.Item.level;
                }
            }
        }
    }
    private void LateUpdate()
    {
        Enemy.Instance.TrunText.text = currentTrun + " / " + MaxTrun;

        if (!IsBusy && !Result.activeSelf)
        {
            if (Enemy.Instance.IsDie)
            {
                Result.SetActive(true);
                BattleResult.Instance.GameResult(true);
                return;
            }
            if (currentTrun == MaxTrun || Player.Instance.IsDie)
            {
                Result.SetActive(true);
                BattleResult.Instance.GameResult(false);
                return;
            }
        }
    }

    #region 交換
    public async void ChangeTile(Tile originTile, Tile targetTile)
    {
        if (IsBusy || Enemy.Instance.BusyTime > 0) { return; }
        IsBusy = true;
        _moveTiles.Clear();
        _moveTiles.Add(originTile);
        _moveTiles.Add(targetTile);
        await Swap(originTile, targetTile);

        if (targetTile.Item != null && CanPop())
        {
            combos = 0;
            await Pop();
            if (!Enemy.Instance.IsDie)
            {
                if (currentTrun % 4 == 0)
                {
                    NewDebuffTiles();
                }

                if (currentTrun % 3 == 0)
                {
                    Enemy.Instance.Attack();
                }
            }
            currentTrun++;
            SkillManager.UpdateSkillTime();
            DoPoison();
            await Task.Delay(1000);
        }
        else
        {
            await Swap(targetTile, originTile);
        }
        if (MaxCombos < combos) { MaxCombos = combos; }
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
        Image frame1 = tile1.frame;
        Image frame2 = tile2.frame;
        Image rune1 = tile1.rune;
        Image rune2 = tile2.rune;
        Image debuffIcon1 = tile1.debuffIcon;
        Image debuffIcon2 = tile2.debuffIcon;
        Animator animator1 = tile1.animator;
        Animator animator2 = tile2.animator;

        Transform transform1 = frame1.transform;
        Transform transform2 = frame2.transform;
        Transform transform11 = rune1.transform;
        Transform transform22 = rune2.transform;

        Sequence sequence = DOTween.Sequence();

        sequence.Join(transform1.DOMove(transform2.position, TweenDuration))
                .Join(transform2.DOMove(transform1.position, TweenDuration))
                .Join(transform11.DOMove(transform22.position, TweenDuration))
                .Join(transform22.DOMove(transform11.position, TweenDuration));
        await sequence.Play()
                    .AsyncWaitForCompletion();

        transform1.SetParent(tile2.transform);
        transform2.SetParent(tile1.transform);

        tile1.frame = frame2;
        tile2.frame = frame1;
        tile1.rune = rune2;
        tile2.rune = rune1;
        tile1.debuffIcon = debuffIcon2;
        tile2.debuffIcon = debuffIcon1;
        tile1.animator = animator2;
        tile2.animator = animator1;

        Item tileItem = tile1.Item;
        tile1.Item = tile2.Item;
        tile2.Item = tileItem;
    }
    #endregion

    #region 破壞
    public async void BreakTile(Tile tile)
    {
        if (IsBusy) { return; }
        IsBusy = true;
        SkillManager.SetBreakSkill(false);
        //變大1.5倍
        Sequence Sequence = DOTween.Sequence();
        Sequence.Join(tile.rune.transform.DOScale(Vector3.one * 1.5f, TweenDuration));
        await Sequence.Play().AsyncWaitForCompletion();

        //縮到最小 消失效果
        Sequence deflateSequence = DOTween.Sequence();
        deflateSequence.Join(tile.frame.transform.DOScale(Vector3.zero, FallDuration));
        deflateSequence.Join(tile.rune.transform.DOScale(Vector3.zero, FallDuration));
        AudioManager.PlaySE(SEEnum.Pop);
        await deflateSequence.Play()
                            .AsyncWaitForCompletion();
        //清空圖片 設定透明 還原大小
        Sequence inflateSequence = DOTween.Sequence();
        tile.Item = null;
        tile.SetIconAlpha(false);
        inflateSequence.Join(tile.frame.transform.DOScale(Vector3.one, TweenDuration));
        inflateSequence.Join(tile.rune.transform.DOScale(Vector3.one, TweenDuration));
        await inflateSequence.Play()
                            .AsyncWaitForCompletion();
        DoDebuff();
        await FallTile();
        if (CanPop())
        {
            await Pop();
        }
        IsBusy = false;
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
                            Item newItem = ItemDatabase.Items[Random.Range(0, ItemDatabase.Items.Count)];
                            Tiles[x, y].Item = Instantiate(newItem);
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
        DoDebuff();
        List<Tile> connectedTiles = new List<Tile>();
        List<Tile> bonusConnected = new List<Tile>();
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
                    if (_tile.debuffIndex != ItemDebuffEnum.Non) { continue; }
                    if (_tile.Item.level == 1)
                    {
                        if (_tile.Item.isEffectH)
                        {
                            for (int i = 0; i < Width; i++)
                            {
                                bonusConnected.Add(Tiles[i, _tile.y]);
                                //connectedTiles.Add(Tiles[i, _tile.y]);
                            }
                            _hitList.Add((tile.Item, 8, _tile.transform.position));
                        }
                        else
                        {
                            for (int i = 0; i < Height; i++)
                            {
                                bonusConnected.Add(Tiles[_tile.x, i]);
                                //connectedTiles.Add(Tiles[_tile.x, i]);
                            }
                            _hitList.Add((tile.Item, 7, _tile.transform.position));
                        }
                    }
                    else if (_tile.Item.level == 2)
                    {
                        for (int i = 0; i < Width; i++)
                        {
                            bonusConnected.Add(Tiles[i, _tile.y]);
                            //connectedTiles.Add(Tiles[i, _tile.y]);
                        }
                        for (int i = 0; i < Height; i++)
                        {
                            bonusConnected.Add(Tiles[_tile.x, i]);
                            //connectedTiles.Add(Tiles[_tile.x, i]);
                        }
                        _hitList.Add((tile.Item, 14, _tile.transform.position));
                    }
                }
                //一般的消除
                if (tiles.Count() > 3) //3消以上 產生BounsTile
                {
                    bool _isCross = false;
                    foreach (Tile _tile in tiles)
                    {
                        List<Tile> tilesH = new List<Tile>();
                        List<Tile> tilesV = new List<Tile>();
                        tilesH.AddRange(_tile.GetConnectedTiles(true));
                        tilesV.AddRange(_tile.GetConnectedTiles(false));
                        //交叉的 Tile 產生 BonusTile
                        if ((tilesH.Count > 2 && tilesV.Count > 2))
                        {
                            tile = _tile;
                            _isCross = true;
                            break;
                        }
                    }
                    if (!_isCross)
                    {   //移動的 Tile 產生 BonusTile
                        foreach (Tile _tile in tiles)
                        {
                            if (_moveTiles.IndexOf(_tile) > -1)
                            {
                                tile = _tile;
                                break;
                            }
                        }
                    }
                    /*
                    //讓有移動的Tile 優先產生BonusTile
                    foreach (Tile _tile in tiles)
                    {   
                        if (_moveTiles.IndexOf(_tile) > -1)
                        {
                            tile = _tile;
                            break;
                        }
                    }
                    */
                    if (tiles.Count() == 4)
                        tile.Item.bonusLevel = 1;
                    if (tiles.Count() > 4)
                        tile.Item.bonusLevel = 2;
                    _bonusTiles.Add(tile);
                }
                _hitList.Add((tile.Item, tiles.Count(), tile.transform.position));
                connectedTiles.AddRange(tiles);
                connectedTiles.AddRange(bonusConnected);
                connectedTiles = connectedTiles.Distinct(new TileCompare()).ToList();
                _moveTiles.Clear();
            }
        }
        //變大1.5倍
        Sequence Sequence = DOTween.Sequence();
        foreach (Tile connectedTile in connectedTiles)
        {
            Sequence.Join(connectedTile.rune.transform.DOScale(Vector3.one * 1.5f, TweenDuration));
            connectedTile.rune.material = connectedTile.Item.GlowMaterial;
        }
        await Sequence.Play().AsyncWaitForCompletion();

        //縮到最小 消失效果
        Sequence deflateSequence = DOTween.Sequence();
        foreach (Tile connectedTile in connectedTiles)
        {
            deflateSequence.Join(connectedTile.frame.transform.DOScale(Vector3.zero, FallDuration));
            deflateSequence.Join(connectedTile.rune.transform.DOScale(Vector3.zero, FallDuration));
            connectedTile.rune.material = null;
        }
        AudioManager.PlaySE(SEEnum.Pop);
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
            inflateSequence.Join(connectedTile.frame.transform.DOScale(Vector3.one, TweenDuration));
            inflateSequence.Join(connectedTile.rune.transform.DOScale(Vector3.one, TweenDuration));
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

            Image frame1 = tile1.frame;
            Image frame2 = tile2.frame;
            Image rune1 = tile1.rune;
            Image rune2 = tile2.rune;
            Image debuffIcon1 = tile1.debuffIcon;
            Image debuffIcon2 = tile2.debuffIcon;
            Animator animator1 = tile1.animator;
            Animator animator2 = tile2.animator;

            Transform transform1 = frame1.transform;
            Transform transform2 = frame2.transform;
            Transform transform11 = rune1.transform;
            Transform transform22 = rune2.transform;

            sequence.Join(transform1.DOMove(transform2.position, FallDuration))
                    .Join(transform2.DOMove(transform1.position, FallDuration))
                    .Join(transform11.DOMove(transform22.position, FallDuration))
                    .Join(transform22.DOMove(transform11.position, FallDuration));
            //Debug.Log(_tile.name);

            transform1.SetParent(tile2.transform);
            transform2.SetParent(tile1.transform);

            tile1.frame = frame2;
            tile2.frame = frame1;
            tile1.rune = rune2;
            tile2.rune = rune1;
            tile1.debuffIcon = debuffIcon2;
            tile2.debuffIcon = debuffIcon1;
            tile1.animator = animator2;
            tile2.animator = animator1;

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
            Item newItem = ItemDatabase.Items[Random.Range(0, ItemDatabase.Items.Count)];
            _tile.Item = Instantiate(newItem);
            _tile.Item.bonusLevel = 0; // Bonus產出的會回寫到物件上 要改回0
            _tile.debuffIndex = ItemDebuffEnum.Non;
            sequence.Join(_tile.frame.transform.DOScale(Vector3.one, 0));
            sequence.Join(_tile.rune.transform.DOScale(Vector3.one, 0));
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
        Item newItem = new Item();
        foreach (Tile _tile in Tiles)
        {
            if (_tile.Item.color == ItemColorEnum.Red)
            {
                if (_tile.Item.bonusLevel == 2)
                    newItem = ItemDatabase.RedPlus;
                else if (_tile.Item.bonusLevel == 1)
                    newItem = ItemDatabase.RedBonus[Random.Range(0, ItemDatabase.RedBonus.Count)];
            }
            else if (_tile.Item.color == ItemColorEnum.Blue)
            {
                if (_tile.Item.bonusLevel == 2)
                    newItem = ItemDatabase.BluePlus;
                else if (_tile.Item.bonusLevel == 1)
                    newItem = ItemDatabase.BlueBonus[Random.Range(0, ItemDatabase.BlueBonus.Count)];
            }
            else if (_tile.Item.color == ItemColorEnum.Green)
            {
                if (_tile.Item.bonusLevel == 2)
                    newItem = ItemDatabase.GreenPlus;
                else if (_tile.Item.bonusLevel == 1)
                    newItem = ItemDatabase.GreenBonus[Random.Range(0, ItemDatabase.GreenBonus.Count)];
            }
            else if (_tile.Item.color == ItemColorEnum.Yellow)
            {
                if (_tile.Item.bonusLevel == 2)
                    newItem = ItemDatabase.YellowPlus;
                else if (_tile.Item.bonusLevel == 1)
                    newItem = ItemDatabase.YellowBonus[Random.Range(0, ItemDatabase.YellowBonus.Count)];
            }
            _tile.Item = Instantiate(newItem);
            sequence.Join(_tile.frame.transform.DOScale(Vector3.one, TweenDuration));
            sequence.Join(_tile.rune.transform.DOScale(Vector3.one, TweenDuration));
            _tile.SetIconAlpha(true);
            _bonusTiles.Remove(_tile);
        }
        await sequence.Play()
            .AsyncWaitForCompletion();
    }
    private void NewDebuffTiles()
    {
        AudioManager.PlaySE(SEEnum.Debuff);
        int count = 0;
        int debuffIndex = Random.Range(1, 4);
        while (count < 2)
        {
            int x = Random.Range(0, Width - 1);
            int y = Random.Range(0, Height - 1);
            Item debuffItem = Instantiate(Tiles[x, y].Item);
            if (debuffItem.debuffIndex != ItemDebuffEnum.Non) { continue; }
            debuffItem.debuffIndex = (ItemDebuffEnum)debuffIndex;
            Tiles[x, y].Item = debuffItem;
            count++;
        }
        DoDebuff();
    }
    #endregion

    #region 計算
    private void Calculate()
    {
        List<(Item, int, Vector3)> list = new List<(Item, int, Vector3)>();
        list.AddRange(_hitList);
        float _rate = SkillManager.Instance.ATK;
        foreach ((Item, int, Vector3) _hit in list)
        {
            float _damage = 100 * Random.Range(1.000f, 1.500f);
            combos++;
            //基礎傷害 * 顆數 * 連擊倍率(1 + combos * 0.5) * 被動加成 * Level
            _damage = _damage * _hit.Item2 * (1 + combos * HitBonus) * _rate * (_hit.Item1.level + 1) * SkillManager.Instance.Power * ATKDown;
            Text.text += _hit.Item1.color.ToString() + " = " + (int)_damage + "\n";
            scrollRect.normalizedPosition = Vector2.zero; //Scroll to Bottom
            if (_hit.Item1.color == ItemColorEnum.Green)
            {
                Player.Instance.Hurt((int)(_damage * -1));
            }
            else
            {
                EnemyController.EnemyHurt(_hit.Item3, (int)_damage, _hit.Item1.color);
            }
            _hitList.Remove(_hit);
        }
    }
    private void DoDebuff()
    {
        ATKDown = 1;
        DEFDown = 1;
        Poison = 0;
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                if (Tiles[x, y].debuffIndex == ItemDebuffEnum.ATKDown)
                {
                    ATKDown = 0.8f;
                }
                else if (Tiles[x, y].debuffIndex == ItemDebuffEnum.DEFDown)
                {
                    DEFDown = 1.5f;
                }
                else if (Tiles[x, y].debuffIndex == ItemDebuffEnum.Poison)
                {
                    Poison = 4000;
                }
            }
        }
        SkillManager.SetActiveDebuffs(ItemDebuffEnum.ATKDown, ATKDown < 1);
        SkillManager.SetActiveDebuffs(ItemDebuffEnum.DEFDown, DEFDown > 1);
        SkillManager.SetActiveDebuffs(ItemDebuffEnum.Poison, Poison > 0);
    }
    private void DoPoison()
    {
        if (Poison == 0) { return; }
        Player.Instance.Hurt((int)Poison);
    }
    #endregion

    public void QuitStage()
    {
        Confirmation.ShowDioalog("是否放棄此次戰鬥？",
                () => 
                {
                    Result.SetActive(true);
                    BattleResult.Instance.GameResult(false);
                },
                () => { }
            );
    }
}
