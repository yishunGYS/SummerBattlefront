using System.Collections.Generic;
using Managers;
using Systems;
using UnityEngine;

namespace UI.Gameplay
{
    public class TeamLeftPanel : UIBasePanel
    {
        
        private TeamTopPanel teamTopPanel;

        public override void OpenPanel()
        {
            base.OpenPanel();

            foreach (var data in DataManager.Instance.GetRuntimeSoliderModel().Values)
            {
                var prefabPath = data.uiPrefabPath;
                var prefab = Resources.Load<GameObject>(prefabPath);
                if (prefab==null)
                {
                    continue;
                }
                GameObject card = Instantiate(prefab, transform);
                var uiPlacedCmpt = card.GetComponent<UIPlaced>();
                uiPlacedCmpt.InitInTeamPanel(data);
                uiPlacedCmpt.view.SetCostText(data.cost);
            }
            
            teamTopPanel = FindObjectOfType<TeamTopPanel>();
        }


        public void OnClickBattleStart()
        {
            var battleSoliderData = DataManager.Instance.GetSolidersInBattle();
            
            //battleSoliderData.Add(7); //默认要加炸弹车
            foreach (var soliderId in teamTopPanel.GetSoliderList())
            {
                battleSoliderData.Add(soliderId);
            }
            

            UIManager.Instance.OnCloseTeamPanel();
            UIManager.Instance.OnOpenSoliderPlacePanel();

            PlayerStats.Instance.StartLevel();
        }
    }
}