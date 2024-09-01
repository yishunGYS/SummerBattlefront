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
        public Dictionary<int, SoliderModelBase> RuntimeSoliderModels = new Dictionary<int, SoliderModelBase>();
        
        private Dictionary<int,EnemyModelBase> EnemyBaseModels = new Dictionary<int, EnemyModelBase>();
        
        private List<int> SolidersInBattle = new List<int>();
        private bool isStart;

        public void OnStart()
        {
            //初始有一个角色
            if (!isStart)
            {
                //Test
                RuntimeSoliderModels = SoliderBaseModels;
                //RuntimeSoliderModels.TryAdd(3, SoliderBaseModels[3]);
                isStart = true;
            }
        }

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
        
        public EnemyModelBase GetEnemyDataById(int id)
        {
            EnemyBaseModels.TryGetValue(id,out EnemyModelBase model);
            return model;
        }
    }
}
