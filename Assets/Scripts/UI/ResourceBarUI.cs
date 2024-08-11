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
        // ����OnMoneyChanged�¼�
        PlayerStats.OnMoneyChanged += UpdateResourceBar;

        CreateGrids(PlayerStats.Instance.currentLimit);
    }

    void OnDisable()
    {
        PlayerStats.OnMoneyChanged -= UpdateResourceBar;
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

    void UpdateResourceBar(int currentMoney, int maxMoney)
    {
        // ������޸ı䣬�������ɸ���
        if (grids.Count != maxMoney)
        {
            CreateGrids(maxMoney);
        }

        // ����ÿ�����ӵ����״̬
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
