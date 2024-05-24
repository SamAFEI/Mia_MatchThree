using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "Match3/StageData", order = 1)]
public class StageData : ScriptableObject
{
    public GameObject Character;
    public int MaxHP = 300000;
    public int Id = 0;
    public int ATK = 5000;
    public string AnimName;
    public string StageName;
    public bool IsComplete;
    public StageData PriorStage;
    public Sprite Background;
    public GameObject SlashFX;
    public List<Sprite> CGList = new List<Sprite>();
    public List<Sprite> MosaicCGList = new List<Sprite>();
    public AudioClip HurtClip;
    public AudioClip AttackClip;
    public List<AudioClip> SexClips = new List<AudioClip>();
    public List<AudioClip> SexVoiceClips = new List<AudioClip>();
}
