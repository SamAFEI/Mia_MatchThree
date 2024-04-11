using Assets.Scripts;
using Assets.Scripts.Manager;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillManager : MonoBehaviour, ISaveManager
{
    private static SkillManager instance;
    public static SkillManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<SkillManager>();
            }
            if (instance == null)
            {
                CreateDefault();
            }
            return instance;
        }
    }
    public List<Skill> Skills = new List<Skill>();
    public List<Skill> ActiveSkills = new List<Skill>();
    public List<Sprite> DebuffIcons = new List<Sprite>();
    public List<Sprite> ActiveDebuffs = new List<Sprite>();
    public int MaxHP;
    public float ATK;
    public float Power;
    public int Coin;
    public bool IsBreakSkill { get; private set; }
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
    private void Start()
    {
        Instance.Power = 1;
        SaveManager.LoadGame();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Instance.Coin += 1000;
        }
    }
    private static void CreateDefault()
    {
        GameObject obj = Resources.Load<GameObject>("Prefabs/Manager/SkillManager");
        obj = Instantiate(obj, Vector3.zero, Quaternion.identity);
        instance = obj.GetComponent<SkillManager>();
        Instance.DebuffIcons.Add(null);
        Instance.DebuffIcons.Add(Resources.Load<Sprite>("Sprites/Skills/ATKDown"));
        Instance.DebuffIcons.Add(Resources.Load<Sprite>("Sprites/Skills/DEFDown"));
        Instance.DebuffIcons.Add(Resources.Load<Sprite>("Sprites/Skills/Poison"));
        Instance.Skills.Add(Resources.Load<Skill>("Prefabs/Skills/ATK"));
        Instance.Skills.Add(Resources.Load<Skill>("Prefabs/Skills/Break"));
        Instance.Skills.Add(Resources.Load<Skill>("Prefabs/Skills/Heal"));
        Instance.Skills.Add(Resources.Load<Skill>("Prefabs/Skills/MaxHP"));
        Instance.Skills.Add(Resources.Load<Skill>("Prefabs/Skills/Power"));
        Instance.Coin = 4000;
        InitSkillValue();
    }
    public static void UpdateSkillTime()
    {
        if (Instance.ActiveSkills.Count == 0) { return; }
        List<Skill> skills = new List<Skill>();
        skills.AddRange(Instance.ActiveSkills);
        foreach (Skill skill in skills)
        {
            skill.UpdateSkillTime();
        }
    }
    public static void AddActiveSkills(Skill _skill)
    {
        Instance.ActiveSkills.Add(_skill);
    }
    public static void RemoveActiveSkills(Skill _skill)
    {
        Instance.ActiveSkills.Remove(_skill);
    }
    public static void SetActiveDebuffs(ItemDebuffEnum _debuff, bool _active)
    {
        Sprite sprite = Instance.DebuffIcons[(int)_debuff];
        if (_active && !Instance.ActiveDebuffs.Contains(sprite))
        {
            Instance.ActiveDebuffs.Add(sprite);
        }
        if (!_active && Instance.ActiveDebuffs.Contains(sprite))
        {
            Instance.ActiveDebuffs.Remove(sprite);
        }
    }
    public static void ClearActiveSkills()
    {
        Instance.ActiveSkills.Clear();
    }
    public static void SetCoin(int _value)
    {
        Instance.Coin += _value;
    }
    public static void InitSkillValue()
    {
        List<Skill> _skills = new List<Skill>();
        _skills.AddRange(Instance.Skills);
        foreach (Skill _skill in _skills)
        {
            _skill.Data.Level = 0;
            _skill.Data.SetValue();
        }
    }
    public static void SetBreakSkill(bool _value)
    {
        Instance.IsBreakSkill = _value;
    }

    public void LoadData(GameData _data)
    {
        Instance.Coin = _data.Coin;
        foreach(Skill skill in Instance.Skills)
        {
            SkillStore _store = _data.Skills.Where(x => x.Name == skill.Data.Name).FirstOrDefault();
            if (_store != null)
            {
                skill.Data.Level = _store.Level;
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.Coin = Instance.Coin;
        _data.Skills = new List<SkillStore>();
        foreach (Skill _skill in Instance.Skills)
        {
            _data.Skills.Add(new SkillStore(_skill.Data));
        }
    }
}
