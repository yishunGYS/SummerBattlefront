using System.Collections.Generic;
using Gameplay.Player;
using UnityEngine;
using Utilities;

namespace Managers
{
    public class DataManager : Singleton<DataManager>
    {
        private Dictionary<int,SoliderModelBase> SoliderBaseModels = new Dictionary<int, SoliderModelBase>();

        public Dictionary<int, SoliderModelBase> GetSoliderBaseModels()
        {
            return SoliderBaseModels;
        }
    }
}
