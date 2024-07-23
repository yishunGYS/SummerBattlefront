using System.Collections.Generic;
using Gameplay.Player;
using UnityEngine;
using Utilities;

namespace Managers
{
    public class DataManager : Singleton<DataManager>
    {
        private Dictionary<int,SoliderBaseModel> SoliderBaseModels = new Dictionary<int, SoliderBaseModel>();

        public Dictionary<int, SoliderBaseModel> GetSoliderBaseModels()
        {
            return SoliderBaseModels;
        }
    }
}
