using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UI.Gameplay;
using UnityEngine;
using Utilities;
using UnityEngine.SceneManagement;
using Systems;
using Gameplay.Item;
using UI;

namespace Managers
{
    public class UIManager : Singleton<UIManager>
    {
        public SerializableDictionary<string, string> pathDicts = new SerializableDictionary<string, string>();

        public Dictionary<string, UIBasePanel> panelSpawnDict = new Dictionary<string, UIBasePanel>();
        public Dictionary<string, UIBasePanel> panelOpenDict = new Dictionary<string, UIBasePanel>();

        private Transform UIRoot;

        // 添加 isOpenTeam 变量
        private bool isOpenTeam = false;
        private bool isShowTip;
        protected override void Awake()
        {
            base.Awake();
            if (_instance == this)
            {
                SceneManager.sceneLoaded += OnSceneLoaded; // 订阅场景加载事件
            }
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded; // 取消订阅场景加载事件
        }

        // 场景加载完成后调用
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // 如果加载的是关卡场景，打开 StartTeamingPanel
            if (scene.name.Contains("Level") && !scene.name.Contains("Select")) // 关卡场景名包含 "Level"
            {
                UIRoot = Resources.Load<Transform>("UIPanel/MainCanvas");
                UIRoot = Instantiate(UIRoot);
                RestData();

                
                InitPanels();
                OpenPanel("StartTeamingPanel");

                // 在开始关卡前重置
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.OnLevelStart();
                }
            }

            if (scene.name == "MainMenu")
            {
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.OnMainMenuStart();
                }
            }
        }



        public void OpenTeamPanel()
        {
            if (isOpenTeam)
            {
                ClosePanel("TeamTopPanel");
                ClosePanel("TeamLeftPanel");
                isOpenTeam = false;
            }
            else
            {
                OpenPanel("TeamTopPanel");
                OpenPanel("TeamLeftPanel");
                ClosePanel("StartTeamingPanel");
                isOpenTeam = true;
            }
        }

        public void BackToStart()
        {
            ClosePanel("TeamTopPanel");
            ClosePanel("TeamLeftPanel");
            isOpenTeam = false;
            OpenPanel("StartTeamingPanel");
        }

        public void OpenEndLevelPanel()
        {
            OpenPanel("LevelEndPanel");
        }

        public void OpenLevelFailPanel()
        {
            OpenPanel("LevelFailPanel");
        }

        private void InitPanels()
        {
            foreach (var item in pathDicts)
            {
                var prefab = Resources.Load<UIBasePanel>(item.Value);
                var temp = Instantiate(prefab, UIRoot);
                panelSpawnDict.Add(item.Key, temp);
            }

            foreach (var item in panelSpawnDict)
            {
                item.Value.Init();
            }
            
            Transform hoverPanel = panelSpawnDict["UnitHoverPanel"].transform;
            hoverPanel.SetSiblingIndex(hoverPanel.parent.childCount-1);
        }

        public void OpenPanel(string name)
        {

            if (panelOpenDict.ContainsKey(name))
            {
                print($"{name}已经打开过，不需要再次打开");
                return;
            }

            var tempPanel = panelSpawnDict[name];
            panelOpenDict.Add(name, tempPanel);
            tempPanel.OpenPanel();
        }

        public void ClosePanel(string name)
        {
            if (!panelOpenDict.TryGetValue(name, out UIBasePanel panel))
            {
                print($"{name}未打开,不需要再次关闭");
                return;
            }

            panel.ClosePanel();
            panelOpenDict.Remove(name);
        }

        public void OnCloseTeamPanel()
        {
            ClosePanel("TeamTopPanel");
            ClosePanel("TeamLeftPanel");
            isOpenTeam = false;
        }

        public void OnOpenSoliderPlacePanel()
        {
            OpenPanel("SpawnSoliderPanel");
        }
        

        public void OnHoverUIPlaced(Vector3 position,string soliderName,string des)
        {
            OpenPanel("UnitHoverPanel");
            UnitHoverPanel hoverPanel = panelOpenDict["UnitHoverPanel"] as UnitHoverPanel;
            if (hoverPanel != null) hoverPanel.OnHoverEnter(position,soliderName,des);
        }

        public void OnHoverUIExit()
        {
            ClosePanel("UnitHoverPanel");
        }

        public void OnOpenResourceLeftPanel()
        {
            OpenPanel("TimeLeftPanel");
        }

        // public void OnUpdateTimeLeftPanel(float leftTime)
        // {
        //     TimeLeftPanel timeLeftPanel = panelOpenDict["TimeLeftPanel"] as TimeLeftPanel;
        //     if (timeLeftPanel != null) timeLeftPanel.UpdateTime(leftTime);
        // }

        public void OnUpdateResourceLeftPanel(float leftMoney)
        {
            TimeLeftPanel timeLeftPanel = panelOpenDict["TimeLeftPanel"] as TimeLeftPanel;
            if (timeLeftPanel != null) timeLeftPanel.UpdateTime(leftMoney);
        }

        public void OnCloseTimeLeftPanel()
        {
            ClosePanel("TimeLeftPanel");
        }


        public async Task OnShowTipPanel(string texts,int delayTime)
        {
            if (!isShowTip)
            {
                OpenPanel("TipPanel");
                TipPanel tipPanel = panelOpenDict["TipPanel"] as TipPanel;
                if (tipPanel != null) tipPanel.OnShowText(texts);
                isShowTip = false;
                
                await Task.Delay(delayTime);
                ClosePanel("TipPanel");
            }
        }
        
        
        public void OnOpenResourcePanel()
        {
            OpenPanel("ResourceBarPanel");
        }

        public void OnUpdateResourcePanel()
        {
            ResourceBarPanel resourceBarPanel = panelOpenDict["ResourceBarPanel"] as ResourceBarPanel;
            if (resourceBarPanel != null) resourceBarPanel.OnUpdateResource();
        }

        public void OnCloseResourcePanel()
        {
            ClosePanel("ResourceBarPanel");
        }



        public void OnShowEduPanel()
        {
            OpenPanel("EduPanel");
        }

        public void OnChangeEduPanelText()
        {
            EduPanel eduPanel = panelOpenDict["EduPanel"] as EduPanel;
            if (eduPanel != null) eduPanel.OnDoRightThing();
        }
        public void OnChangeEduState(EduState state)
        {
            EduPanel eduPanel = panelOpenDict["EduPanel"] as EduPanel;
            if (eduPanel != null) eduPanel.curState = state;
        }
        
        public void OnCloseEduPanel()
        {
            ClosePanel("EduPanel");
        }


        public void OnReachMax()
        {
            ResourceBarPanel resourceBarPanel = panelOpenDict["ResourceBarPanel"] as ResourceBarPanel;
            if (resourceBarPanel != null) resourceBarPanel.OnReachMaxShow();
        }

        public void OnNotReachMax()
        {
            ResourceBarPanel resourceBarPanel = panelOpenDict["ResourceBarPanel"] as ResourceBarPanel;
            if (resourceBarPanel != null) resourceBarPanel.OnReachMaxHide();
        }

        private void RestData()
        {
            panelSpawnDict.Clear();
            panelOpenDict.Clear();
            isOpenTeam = false;
        }
    }
}
