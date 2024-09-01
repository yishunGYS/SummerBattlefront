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
            CreateGrids(Mathf.CeilToInt(PlayerStats.Instance.currentLimit)); // 向上取整创建格子
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
            // 清除现有的格子
            foreach (Transform child in container)
            {
                Destroy(child.gameObject);
            }
            grids.Clear();

            // 根据上限创建新的格子
            for (int i = 0; i < maxMoney; i++)
            {
                GameObject newGrid = Instantiate(gridPrefab, container);
                grids.Add(newGrid.GetComponent<Image>());
            }
        }

        void UpdateResourceBar(float currentMoney, int maxMoney)
        {
            // 如果上限改变，重新生成格子
            if (grids.Count != Mathf.CeilToInt(maxMoney))
            {
                CreateGrids(Mathf.CeilToInt(maxMoney));
            }

            // 更新每个格子的填充状态
            for (int i = 0; i < grids.Count; i++)
            {
                float fillAmount = currentMoney - i;

                if (fillAmount >= 1f)
                {
                    grids[i].fillAmount = 1f;  // 完全填充
                }
                else if (fillAmount > 0f)
                {
                    grids[i].fillAmount = fillAmount;  // 部分填充
                }
                else
                {
                    grids[i].fillAmount = 0f;  // 没有填充
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
