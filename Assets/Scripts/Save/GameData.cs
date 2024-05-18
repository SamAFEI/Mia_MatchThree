using System.Collections.Generic;

namespace Assets.Scripts
{
    [System.Serializable]
    public class GameData
    {
        public int Coin;
        public StageStore CurrentStage;
        public List<StageStore> Stages;
        public List<SkillStore> Skills;
        public PlayerSettingStore PlayerSetting;

        public GameData()
        {
        }
    }
    [System.Serializable]
    public class StageStore
    {
        public string StageName;
        public bool IsComplete;
        public StageStore(StageData _stageData)
        {
            StageName = _stageData.StageName;
            IsComplete = _stageData.IsComplete;
        }
    }
    [System.Serializable]
    public class SkillStore
    {
        public string Name;
        public int Level;
        public SkillStore(BasisSkill _skillData)
        {
            Name = _skillData.Name;
            Level = _skillData.Level;
        }
    }
    [System.Serializable]
    public class PlayerSettingStore
    {
        public int ResolutionWidth;
        public int ResolutionHeight;
        public bool IsFullScreen;
        public int LanguageIndex;
        public float BGMVolume;
        public float SEVolume;
        public float VoiceVolume;
        public PlayerSettingStore()
        {
        }
    }
}
