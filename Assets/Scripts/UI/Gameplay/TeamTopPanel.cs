using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace UI.Gameplay
{
    public class TeamTopPanel : UIBasePanel
    {
        public int maxTeamCount = 7;
        private int curSelectCount;

        private Dictionary<int,UIPlaced> spawnDicts = new Dictionary<int,UIPlaced>();

        public bool isTeamCancelEdued;
        public override void OpenPanel()
        {
            base.OpenPanel();
            DataManager.Instance.ClearSolidersInBattle();
        }

        public bool CheckCanPlaceInTopPanel()
        {
            return curSelectCount < maxTeamCount;
        }

        public void SpawnCardInTopPanel(int id,UIPlaced connectCard)
        {
            var data = DataManager.Instance.GetRuntimeSoliderDataById(id);
            var prefabPath = data.uiPrefabPath;
            var prefab = Resources.Load<GameObject>(prefabPath);
            GameObject card = Instantiate(prefab, transform);
            var uiPlacedCmpt = card.GetComponent<UIPlaced>();
            uiPlacedCmpt.InitInTeamPanel(data);
            uiPlacedCmpt.connectCardInLeftPanel = connectCard;
            uiPlacedCmpt.view.SetCostText(data.cost);
            spawnDicts[id] = uiPlacedCmpt;
            
            IncreaseSelectCount();
        }

        private void ReduceSelectCount()
        {
            if (curSelectCount>0)
            {
                curSelectCount--;
            }
        }

        private void IncreaseSelectCount()
        {
            curSelectCount++;
        }

        public void DestroySpawnedCard(int id)
        {
            if (spawnDicts.ContainsKey(id))
            {
                Destroy(spawnDicts[id].gameObject);
                spawnDicts.Remove(id);
                ReduceSelectCount();
            }
        }


        public List<int> GetSoliderList()
        {
            var tempList = new List<int>();
            foreach (var id in spawnDicts.Keys)
            {
                tempList.Add(id);
            }

            return tempList;
        }
    }
}