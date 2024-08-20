using Managers;
using Systems.Level;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Gameplay
{
    public class StartTeamingPanel : UIBasePanel
    {
        private Button StartTeamingButton;

        void Start()
        {
            InitializeButton();
        }

        void InitializeButton()
        {
            StartTeamingButton = GetComponentInChildren<Button>();

            if (StartTeamingButton != null)
            {
                StartTeamingButton.onClick.AddListener(StartTeamingPanelClicked);
            }
            else
            {
                Debug.LogError("δ�ҵ�StartTeaming��ť��");
            }
        }

        // ��ť����¼�������
        void StartTeamingPanelClicked()
        {
            UIManager.Instance.OpenTeamPanel();
        }
    }
}
