using Managers;
using Systems.Edu;
using Systems.Level;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Gameplay
{
    public class StartTeamingPanel : UIBasePanel
    {
        private Button StartTeamingButton;
        private bool isClickEdued = false;
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
                Debug.LogError("未找到StartTeaming按钮！");
            }
        }

        // 按钮点击事件处理函数
        void StartTeamingPanelClicked()
        {
            UIManager.Instance.OpenTeamPanel();
            if (FindObjectOfType<EduSystem>() && !isClickEdued)
            {
                UIManager.Instance.OnChangeEduPanelText();
                isClickEdued = true;
            }
            UIManager.Instance.OnOpenResourceLeftPanel();
        }
    }
}
