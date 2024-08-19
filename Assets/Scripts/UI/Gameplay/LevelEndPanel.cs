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
                Debug.LogError("δ�ҵ������ؿ���ť��");
            }
        }

        // ��ť����¼�������
        void OnEndLevelButtonClicked()
        {
            LevelManager.Instance.LevelEnd();
        }
    }
}
