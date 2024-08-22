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
        CreateGrids(Mathf.CeilToInt(PlayerStats.Instance.currentLimit)); // ����ȡ����������
    }

    void Update()
    {
        // ��ÿһ֡������Դ��
        UpdateResourceBar(PlayerStats.Instance.CurrentMoney(), PlayerStats.Instance.currentLimit);
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
}
