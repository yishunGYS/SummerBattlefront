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

        // ��ť����¼�������
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
