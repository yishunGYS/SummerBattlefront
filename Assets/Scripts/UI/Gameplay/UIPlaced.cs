using DG.Tweening;
using Gameplay.Player;
using Managers;
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


    public class UIPlaced : MonoBehaviour, IPointerClickHandler
    {
        private SoliderModelBase soliderData;

        private TeamLeftPanel teamLeftPanel;
        private TeamTopPanel teamTopPanel;
        private SpawnSoliderPanel spawnSoliderPanel;

        private Image img;
        private Color originalColor;
        private Color darkenedColor;

        public UIPlaced connectCardInLeftPanel;
        private CardState curState;

        public void InitInTeamPanel(SoliderModelBase data)
        {
            teamLeftPanel = FindObjectOfType<TeamLeftPanel>();
            teamTopPanel = FindObjectOfType<TeamTopPanel>();
            soliderData = data;
            img = GetComponent<Image>();
            originalColor = img.color;
            darkenedColor = originalColor * 0.1f;

            curState = CardState.InTeamPanel;
        }

        public void InitInGamePanel(SoliderModelBase data)
        {
            spawnSoliderPanel = FindObjectOfType<SpawnSoliderPanel>();
            soliderData = data;

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
                if (teamTopPanel.CheckCanPlaceInTopPanel())
                {
                    teamTopPanel.SpawnCardInTopPanel(soliderData.soliderId, this);
                    //自己变暗
                    img.color = darkenedColor;
                }
            }
            else if (curState == CardState.InTeamPanel &&
                     RectTransformUtility.RectangleContainsScreenPoint(teamTopPanel.GetComponent<RectTransform>(),
                         Input.mousePosition))
            {
                //在上边栏时
                connectCardInLeftPanel.img.color = originalColor;
                teamTopPanel.DestroySpawnedCard(soliderData.soliderId);
            }
            else if (curState == CardState.InGamePanel &&
                     RectTransformUtility.RectangleContainsScreenPoint(spawnSoliderPanel.GetComponent<RectTransform>(),
                         Input.mousePosition))
            {
                //在派兵栏时 派兵逻辑
                SpawnManager.Instance.ChangeSelectSolider(soliderData.soliderId);
                spawnSoliderPanel.OnSelectCard(GetComponent<RectTransform>());
            }
        }
    }
}