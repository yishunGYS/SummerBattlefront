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
        // 监听OnMoneyChanged事件
        PlayerStats.OnMoneyChanged += UpdateResourceBar;

        CreateGrids(PlayerStats.Instance.currentLimit);
    }

    void OnDisable()
    {
        PlayerStats.OnMoneyChanged -= UpdateResourceBar;
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

    void UpdateResourceBar(int currentMoney, int maxMoney)
    {
        // 如果上限改变，重新生成格子
        if (grids.Count != maxMoney)
        {
            CreateGrids(maxMoney);
        }

        // 更新每个格子的填充状态
        for (int i = 0; i < grids.Count; i++)
        {
            if (i < currentMoney)
            {
                grids[i].fillAmount = 1f;
            }
            else
            {
                grids[i].fillAmount = 0f;
            }
        }
    }
}
