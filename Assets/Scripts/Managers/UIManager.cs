using System;
using System.Collections.Generic;
using TMPro;
using UI.Gameplay;
using UnityEngine;
using Utilities;

namespace Managers
{
    public class UIManager : Singleton<UIManager>
    {
        public SerializableDictionary<string, string> pathDicts = new SerializableDictionary<string, string>();
        
        private Dictionary<string,UIBasePanel> panelSpawnDict = new Dictionary<string, UIBasePanel>();
        private Dictionary<string, UIBasePanel> panelOpenDict = new Dictionary<string, UIBasePanel>();
        
        private Transform UIRoot;
        
        //public TMP_Text TipText;
        private bool isOpenTeam;
        public void OnStart()
        {
            UIRoot = Resources.Load<Transform>("UIPanel/MainCanvas");
            UIRoot = Instantiate(UIRoot);
                
            InitPanels();

            OpenPanel("StartTeamingPanel");
            //TipText.alpha = 0;
        }

        private void Update()
        {
            //if (Input.GetKeyDown(KeyCode.T))
            //{

            //}

            // if (Input.GetKeyDown(KeyCode.P))
            // {
            //     OpenPanel("SpawnSoliderPanel");
            // }
        }

        public void OpenTeamPanel()
        {
            if (isOpenTeam)
            {
                ClosePanel("TeamTopPanel");
                ClosePanel("TeamLeftPanel");
                isOpenTeam = true;
            }
            else
            {
                OpenPanel("TeamTopPanel");
                OpenPanel("TeamLeftPanel");
                ClosePanel("StartTeamingPanel");
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
                panelSpawnDict.Add(item.Key,temp);
            }


            foreach (var item in panelSpawnDict)
            {
                item.Value.Init();
            }
        }
        
        public void OpenPanel(string name)
        {
            //看panel是否已经打开过
            if (panelOpenDict.ContainsKey(name))
            {
                print($"{name}已经打开过，不需要再次打开");
                return;
            }

            var tempPanel = panelSpawnDict[name];
            panelOpenDict.Add(name,tempPanel);
            tempPanel.OpenPanel();
            
        }


        public void ClosePanel(string name)
        {
            if (!panelOpenDict.TryGetValue(name,out UIBasePanel panel))
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
        }

        public void OnOpenSoliderPlacePanel()
        {
            OpenPanel("SpawnSoliderPanel");
        }
    }
}
