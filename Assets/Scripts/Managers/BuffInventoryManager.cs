using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Managers
{
    public class BuffInventoryManager : Singleton<BuffInventoryManager>
    {
        public List<BuffInfo> buffInfos = new List<BuffInfo>();
        
        public BuffInfo GetBuffById(int id)
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