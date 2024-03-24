using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
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
    public bool IsBreakSkill {  get; private set; }
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
    }
    private static void CreateDefault()
    {
        GameObject obj = Resources.Load<GameObject>("Prefabs/Manager/SkillManager");
        obj = Instantiate(obj, Vector3.zero, Quaternion.identity);
        instance = obj.GetComponent<SkillManager>();
        Instance.Coin = 4000;
        Instance.DebuffIcons.Add(null);
        Instance.DebuffIcons.Add(Resources.Load<Sprite>("Sprites/Skills/ATKDown"));
        Instance.DebuffIcons.Add(Resources.Load<Sprite>("Sprites/Skills/DEFDown"));
        Instance.DebuffIcons.Add(Resources.Load<Sprite>("Sprites/Skills/Poison"));
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
    public static void SetActiveDebuffs(ItemDebuffEnum _debuff,bool _active)
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
    public static void InitSkillLevel()
    {
        if (Instance.Skills.Count > 0) { return; }
        List<Skill> _skills = GameObject.FindObjectsOfType<Skill>().ToList();
        foreach (Skill _skill in _skills)
        {
            _skill.Data.Level = 0;
            _skill.Data.SetValue();
            GameObject obj = Instantiate(_skill.GetComponent<Skill>().gameObject, Vector3.zero, Quaternion.identity, Instance.transform);
            obj.name = _skill.name;
            Instance.Skills.Add(obj.GetComponent<Skill>());
        }
    }
    public static void SetBreakSkill(bool _value)
    {
        Instance.IsBreakSkill = _value;
    }
}
