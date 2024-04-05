using System.Collections.Generic;

namespace Assets.Scripts
{
    [System.Serializable]
    public class GameData
    {
        public int Coin;
        public List<Skill> Skills;
        public Stage CurrentStage;
        public List<Stage> Stages;

        public GameData() 
        {
        }
    }
}
