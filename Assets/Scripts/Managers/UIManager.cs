using System;
using System.Collections.Generic;
using TMPro;
using UI.Gameplay;
using UnityEngine;
using Utilities;
using UnityEngine.SceneManagement;
using Systems;

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

        private void Awake()
        {
            base.Awake();
            SceneManager.sceneLoaded += OnSceneLoaded; // 订阅场景加载事件
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

                panelSpawnDict.Clear();
                panelOpenDict.Clear();
                isOpenTeam = false;
                
                InitPanels();
                OpenPanel("StartTeamingPanel");

                // 在开始关卡前重置
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.OnLevelStart();
                }
            }
        }

        public void OnStart()
        {
            // 不再需要这里的初始化，因为场景加载时会进行初始化
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


        private void RestData()
        {
            
        }
    }
}
