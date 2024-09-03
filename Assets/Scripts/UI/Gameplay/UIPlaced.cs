using DG.Tweening;
using Gameplay.Player;
using Managers;
using Systems.Edu;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Gameplay
{
    public enum CardState
    {
        InTeamPanel,
        InGamePanel
    }

    [RequireComponent(typeof(UIPlacedView))]
    public class UIPlaced : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private SoliderModelBase soliderData;

        private TeamLeftPanel teamLeftPanel;
        private TeamTopPanel teamTopPanel;
        private SpawnSoliderPanel spawnSoliderPanel;

        [HideInInspector]public UIPlacedView view;
        [HideInInspector]public UIPlaced connectCardInLeftPanel;
        private CardState curState;

        private bool isClickInLeftPanel;
        
        public void InitInTeamPanel(SoliderModelBase data)
        {
            teamLeftPanel = FindObjectOfType<TeamLeftPanel>();
            teamTopPanel = FindObjectOfType<TeamTopPanel>();
            soliderData = data;
            view = GetComponent<UIPlacedView>();
            view.OnInit();
            curState = CardState.InTeamPanel;
        }

        public void InitInGamePanel(SoliderModelBase data)
        {
            spawnSoliderPanel = FindObjectOfType<SpawnSoliderPanel>();
            soliderData = data;
            view = GetComponent<UIPlacedView>();
            view.OnInit();
            curState = CardState.InGamePanel;
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("OnMouseDown");

            if (curState == CardState.InTeamPanel &&
                RectTransformUtility.RectangleContainsScreenPoint(teamLeftPanel.GetComponent<RectTransform>(),
                    Input.mousePosition))
            {
                //在左边栏时
                if (teamTopPanel.CheckCanPlaceInTopPanel()&&!isClickInLeftPanel)
                {
                    teamTopPanel.SpawnCardInTopPanel(soliderData.soliderId, this);
                    //自己变暗
                    view.ChangeUIColor(true);
                    isClickInLeftPanel = true;
                }
                //Edu
                if (FindObjectOfType<EduSystem>()&&! teamLeftPanel.isTeamClickEdued)
                {
                    UIManager.Instance.OnChangeEduPanelText();
                    teamLeftPanel.isTeamClickEdued = true;
                }
            }
            else if (curState == CardState.InTeamPanel &&
                     RectTransformUtility.RectangleContainsScreenPoint(teamTopPanel.GetComponent<RectTransform>(),
                         Input.mousePosition))
            {
                connectCardInLeftPanel.view.ChangeUIColor(false);
                connectCardInLeftPanel.isClickInLeftPanel = false;
                teamTopPanel.DestroySpawnedCard(soliderData.soliderId);
                
                //Edu
                //在上边栏时
                if (FindObjectOfType<EduSystem>() && !teamTopPanel.isTeamCancelEdued)
                {
                    UIManager.Instance.OnChangeEduPanelText();
                    teamTopPanel.isTeamCancelEdued = true;
                }
            }
            else if (curState == CardState.InGamePanel &&
                     RectTransformUtility.RectangleContainsScreenPoint(spawnSoliderPanel.GetComponent<RectTransform>(),
                         Input.mousePosition))
            {
                //在派兵栏时 派兵逻辑
                SpawnManager.Instance.ChangeSelectSolider(soliderData.soliderId);
                spawnSoliderPanel.OnSelectCard(view);
                
                
                //Edu
                if (FindObjectOfType<EduSystem>() && !spawnSoliderPanel.isPlaceEdued )
                {
                    var eduPanel = UIManager.Instance.panelSpawnDict["EduPanel"] as EduPanel;
                    if (eduPanel && eduPanel.battleIndex == 4)
                    {
                        UIManager.Instance.OnChangeEduPanelText();
                        spawnSoliderPanel.isPlaceEdued = true;
                    }
                    
                }
            }
        }
        
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            var pos = GetComponent<RectTransform>().position;
            UIManager.Instance.OnHoverUIPlaced(pos,soliderData.soliderName, soliderData.soliderDes);
            //view.OnHoverUI(soliderData.soliderName, soliderData.soliderDes);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            UIManager.Instance.OnHoverUIExit();
            //view.OnExitHover();
            
        }
    }
}