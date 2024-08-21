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

        // ��� isOpenTeam ����
        private bool isOpenTeam = false;

        private void Awake()
        {
            base.Awake();
            SceneManager.sceneLoaded += OnSceneLoaded; // ���ĳ��������¼�
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded; // ȡ�����ĳ��������¼�
        }

        // ����������ɺ����
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // ������ص��ǹؿ��������� StartTeamingPanel
            if (scene.name.Contains("Level") && !scene.name.Contains("Select")) // �ؿ����������� "Level"
            {
                UIRoot = Resources.Load<Transform>("UIPanel/MainCanvas");
                UIRoot = Instantiate(UIRoot);

                panelSpawnDict.Clear();
                panelOpenDict.Clear();
                isOpenTeam = false;
                
                InitPanels();
                OpenPanel("StartTeamingPanel");

                // �ڿ�ʼ�ؿ�ǰ����
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.OnLevelStart();
                }
            }
        }

        public void OnStart()
        {
            // ������Ҫ����ĳ�ʼ������Ϊ��������ʱ����г�ʼ��
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
                print($"{name}�Ѿ��򿪹�������Ҫ�ٴδ�");
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
                print($"{name}δ��,����Ҫ�ٴιر�");
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
