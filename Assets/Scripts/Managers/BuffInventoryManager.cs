using System.Collections.Generic;
using Systems.Buff;
using UnityEngine;
using Utilities;

namespace Managers
{
    public class BuffInventoryManager : Singleton<BuffInventoryManager>
    {
        public List<BuffDataSO> buffInfos = new List<BuffDataSO>();
        
        public BuffDataSO GetBuffById(int id)
        {
            foreach (var item in buffInfos)
            {
                if (item.id == id)
                {
                    Debug.Log("获得Buff: " + item.name);
                    return item;
                }
            }

            return null;
        }
    }
}