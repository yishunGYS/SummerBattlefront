using System;
using Systems.Level;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Gameplay
{
    public class LevelFailPanel : UIBasePanel
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
                endLevelButton.onClick.AddListener(OnFailLevelButtonClicked);
            }
        }

        // 按钮点击事件处理函数
        void OnFailLevelButtonClicked()
        {
            LevelManager.Instance.LevelFail();
        }

        public void OnClickReplay()
        {
            LevelManager.Instance.RePlay();
        }
    }
}
