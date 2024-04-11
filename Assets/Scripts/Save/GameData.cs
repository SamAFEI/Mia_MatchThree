using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    [System.Serializable]
    public class GameData
    {
        public int Coin;
        public StageStore CurrentStage;
        public List<StageStore> Stages;
        public List<SkillStore> Skills;

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
}
