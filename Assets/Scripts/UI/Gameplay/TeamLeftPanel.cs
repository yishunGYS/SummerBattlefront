using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Gameplay
{
    public class TeamLeftPanel : UIBasePanel
    {
        private TeamTopPanel teamTopPanel;

        public Button BackButton;

        // ���ڸ��������ɵİ�ť
        private bool buttonsGenerated = false;

        void Start()
        {
            InitializeButton();

            // �������������¼�
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void InitializeButton()
        {
            if (BackButton != null)
            {
                BackButton.onClick.AddListener(StartTeamingPanelClicked);
            }
            else
            {
                Debug.LogError("δ�ҵ�BackButton��ť��");
            }
        }

        void StartTeamingPanelClicked()
        {
            UIManager.Instance.BackToStart();
        }

        public override void OpenPanel()
        {
            base.OpenPanel();

            // ��鰴ť�Ƿ��Ѿ�����
            if (!buttonsGenerated)
            {
                foreach (var data in DataManager.Instance.GetRuntimeSoliderModel().Values)
                {
                    var prefabPath = data.uiPrefabPath;
                    var prefab = Resources.Load<GameObject>(prefabPath);
                    if (prefab == null)
                    {
                        continue;
                    }
                    GameObject card = Instantiate(prefab, transform);
                    var uiPlacedCmpt = card.GetComponent<UIPlaced>();
                    uiPlacedCmpt.InitInTeamPanel(data);
                    uiPlacedCmpt.view.SetCostText(data.cost);
                }

                // ��ǰ�ť������
                buttonsGenerated = true;
            }

            teamTopPanel = FindObjectOfType<TeamTopPanel>();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            buttonsGenerated = false;
        }

        public void OnClickBattleStart()
        {
            var battleSoliderData = DataManager.Instance.GetSolidersInBattle();
            var soliderLists = teamTopPanel.GetSoliderList();
            if (soliderLists.Count<=0)
            {
                UIManager.Instance.OnShowTipPanel("��ѡ���ս��ɫ");
                return;
            }
            
            foreach (var soliderId in teamTopPanel.GetSoliderList())
            {
                battleSoliderData.Add(soliderId);
            }

            UIManager.Instance.OnCloseTeamPanel();
            UIManager.Instance.OnOpenSoliderPlacePanel();
            SpawnManager.Instance.isLevelStarted = true;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
