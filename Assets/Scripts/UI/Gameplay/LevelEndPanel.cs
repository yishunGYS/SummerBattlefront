using Systems.Level;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Gameplay
{
    public class LevelEndPanel : UIBasePanel
    {
        private Button endLevelButton;

        void Start()
        {
            InitializeButton();
        }

        void InitializeButton()
        {
            endLevelButton = GetComponentInChildren<Button>();

            if (endLevelButton != null)
            {
                endLevelButton.onClick.AddListener(OnEndLevelButtonClicked);
            }
            else
            {
                Debug.LogError("未找到结束关卡按钮！");
            }
        }

        // 按钮点击事件处理函数
        void OnEndLevelButtonClicked()
        {
            LevelManager.Instance.LevelEnd();
        }
    }
}
