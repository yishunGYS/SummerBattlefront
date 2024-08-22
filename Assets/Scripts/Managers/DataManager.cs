using System.Collections.Generic;
using Gameplay.Enemy;
using Gameplay.Player;
using UnityEngine;
using Utilities;

namespace Managers
{
    public class DataManager : Singleton<DataManager>
    {
        public List<int> InitSoliderIds = new List<int>();
        
        private Dictionary<int,SoliderModelBase> SoliderBaseModels = new Dictionary<int, SoliderModelBase>();
        private Dictionary<int, SoliderModelBase> RuntimeSoliderModels = new Dictionary<int, SoliderModelBase>();
        
        private Dictionary<int,EnemyModelBase> EnemyBaseModels = new Dictionary<int, EnemyModelBase>();
        
        private List<int> SolidersInBattle = new List<int>();
        public Dictionary<int, SoliderModelBase> GetSoliderBaseModels()
        {
            return SoliderBaseModels;
        }

        public Dictionary<int, SoliderModelBase> GetRuntimeSoliderModel()
        {
            return RuntimeSoliderModels;
        }

        public Dictionary<int, EnemyModelBase> GetEnemyBaseModels()
        {
            return EnemyBaseModels;
        }


        public List<int> GetSolidersInBattle()
        {
            return SolidersInBattle;
        }

        public void ClearSolidersInBattle()
        {
            SolidersInBattle.Clear();
        }

        public SoliderModelBase GetSoliderDataById(int id) 
        {
            SoliderBaseModels.TryGetValue(id,out SoliderModelBase model);
            return model;
        }
        
        public SoliderModelBase GetRuntimeSoliderDataById(int id) 
        {
            RuntimeSoliderModels.TryGetValue(id,out SoliderModelBase model);
            return model;
        }
    }
}
