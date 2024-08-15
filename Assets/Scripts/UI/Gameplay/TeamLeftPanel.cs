using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace UI.Gameplay
{
    public class TeamLeftPanel : UIBasePanel
    {
        
        private TeamTopPanel teamTopPanel;

        public override void OpenPanel()
        {
            base.OpenPanel();

            foreach (var item in DataManager.Instance.GetRuntimeSoliderModel().Values)
            {
                var prefabPath = item.uiPrefabPath;
                var prefab = Resources.Load<GameObject>(prefabPath);
                GameObject card = Instantiate(prefab, transform);
                var uiPlacedCmpt = card.GetComponent<UIPlaced>();
                uiPlacedCmpt.InitInTeamPanel(item);
            }
            
            teamTopPanel = FindObjectOfType<TeamTopPanel>();
        }


        public void OnClickBattleStart()
        {
            var battleSoliderData = DataManager.Instance.GetSolidersInBattle();
            
            battleSoliderData.Add(7); //默认要加炸弹车
            battleSoliderData.AddRange(teamTopPanel.GetSoliderList());

            UIManager.Instance.OnCloseTeamPanel();
            UIManager.Instance.OnOpenSoliderPlacePanel();
        }
    }
}