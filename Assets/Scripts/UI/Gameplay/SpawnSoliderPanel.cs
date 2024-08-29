using System.Collections.Generic;
using DG.Tweening;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Gameplay
{
    public class SpawnSoliderPanel : UIBasePanel
    {
        private RectTransform curSelectCard;
        public List<UIPlaced> soliderUILists = new List<UIPlaced>();
        public override void OpenPanel()
        {
            base.OpenPanel();
            var soliderList = DataManager.Instance.GetSolidersInBattle();
            foreach (var id in soliderList)
            {
                var soliderData = DataManager.Instance.GetSoliderDataById(id);
                var prefabPath = soliderData.uiPrefabPath;
                var prefab = Resources.Load<GameObject>(prefabPath);
                GameObject card = Instantiate(prefab, transform);
                var uiPlacedCmpt = card.GetComponent<UIPlaced>();
                uiPlacedCmpt.InitInGamePanel(soliderData);
                uiPlacedCmpt.view.SetCostText(soliderData.cost);
                
                soliderUILists.Add(uiPlacedCmpt);
            }

            // 清理面板中已有的士兵卡片
            

        }

        public void OnSelectCard(RectTransform newCard)
        {
            
            if (curSelectCard!=null)
            {
                curSelectCard.GetComponent<Image>().color *= 2f;
            }
            newCard.GetComponent<Image>().color *= 0.5f;
            curSelectCard = newCard;
        }
    }
}