using System.Collections.Generic;
using Gameplay.Enemy;
using Gameplay.Player;
using UnityEngine;
using Utilities;

namespace Managers
{
    public class DataManager : Singleton<DataManager>
    {
        private Dictionary<int,SoliderModelBase> SoliderBaseModels = new Dictionary<int, SoliderModelBase>();
        private Dictionary<int,EnemyModelBase> EnemyBaseModels = new Dictionary<int, EnemyModelBase>();
        public Dictionary<int, SoliderModelBase> GetSoliderBaseModels()
        {
            return SoliderBaseModels;
        }
        
        public Dictionary<int, EnemyModelBase> GetEnemyBaseModels()
        {
            return EnemyBaseModels;
        }


        public SoliderModelBase GetSoliderDataById(int id) 
        {

            SoliderBaseModels.TryGetValue(id,out SoliderModelBase model);
            return model;
        }
    }
}
