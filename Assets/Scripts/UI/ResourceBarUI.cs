using UnityEngine;
using UnityEngine.UI;
using Systems;
using System.Collections.Generic;

public class ResourceBarUI : MonoBehaviour
{
    public GameObject gridPrefab;
    public Transform container;

    private List<Image> grids = new List<Image>();

    void Start()
    {
        CreateGrids(Mathf.CeilToInt(PlayerStats.Instance.currentLimit)); // 向上取整创建格子
    }

    void Update()
    {
        // 在每一帧更新资源条
        UpdateResourceBar(PlayerStats.Instance.CurrentMoney(), PlayerStats.Instance.currentLimit);
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
}
