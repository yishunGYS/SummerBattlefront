using System.Collections.Generic;
using Systems;
using UI.Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ResourceBarPanel : UIBasePanel
    {
        public GameObject gridPrefab;
        public Transform container;

        private List<Image> grids = new List<Image>();

        public bool isStartToRegain;

        public Image normalResource;
        public Image maxResource;
        public override void Init()
        {
            base.Init();
            CreateGrids(Mathf.CeilToInt(PlayerStats.Instance.currentLimit)); // ����ȡ����������
        }

        public override void OpenPanel()
        {
            base.OpenPanel();
            UpdateResourceBar(PlayerStats.Instance.CurrentMoney(), PlayerStats.Instance.currentLimit);
            isStartToRegain = true;
        }

        public void OnUpdateResource()
        {
            if (isStartToRegain)
            {
                UpdateResourceBar(PlayerStats.Instance.CurrentMoney(), PlayerStats.Instance.currentLimit);
            }
        }


        public override void ClosePanel()
        {
            base.ClosePanel();
            isStartToRegain = false;
        }

        void CreateGrids(int maxMoney)
        {
            // ������еĸ���
            foreach (Transform child in container)
            {
                Destroy(child.gameObject);
            }
            grids.Clear();

            // �������޴����µĸ���
            for (int i = 0; i < maxMoney; i++)
            {
                GameObject newGrid = Instantiate(gridPrefab, container);
                grids.Add(newGrid.GetComponent<Image>());
            }
        }

        void UpdateResourceBar(float currentMoney, int maxMoney)
        {
            // ������޸ı䣬�������ɸ���
            if (grids.Count != Mathf.CeilToInt(maxMoney))
            {
                CreateGrids(Mathf.CeilToInt(maxMoney));
            }

            // ����ÿ�����ӵ����״̬
            for (int i = 0; i < grids.Count; i++)
            {
                float fillAmount = currentMoney - i;

                if (fillAmount >= 1f)
                {
                    grids[i].fillAmount = 1f;  // ��ȫ���
                }
                else if (fillAmount > 0f)
                {
                    grids[i].fillAmount = fillAmount;  // �������
                }
                else
                {
                    grids[i].fillAmount = 0f;  // û�����
                }
            }
        }

        public void OnReachMaxShow()
        {
            maxResource.gameObject.SetActive(false);
        }
        
        public void OnReachMaxHide()
        {
            maxResource.gameObject.SetActive(true);
        }
    }
}
