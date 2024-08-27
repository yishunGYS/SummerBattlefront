using System;
using Gameplay.Enemy;
using UnityEngine;

namespace Gameplay.Features
{
    [RequireComponent(typeof(EnemyAgent))]
    public class RangeViewFeature: MonoBehaviour
    {
        private GameObject rangeViewPrefab;
        private EnemyAgent enemyAgent;
        private EnemyModelBase enemyModel;

        public void OnInit()
        {
            
            enemyAgent = GetComponent<EnemyAgent>();
            enemyModel = enemyAgent.enemyModel;
            
            
            rangeViewPrefab = Resources.Load<GameObject>("RangeView/SphereView");
            rangeViewPrefab = Instantiate(rangeViewPrefab, enemyAgent.transform);
            rangeViewPrefab.transform.localScale = new Vector3(enemyModel.attackRange * 2, 0.0001f, enemyModel.attackRange * 2);
            
            var pos = rangeViewPrefab.transform.position - new Vector3(0, 0.5f, 0);
            rangeViewPrefab.transform.position = pos;
            rangeViewPrefab.SetActive(false);
        }

        private void Update()
        {
            DetectMouseClick();
        }

        private void DetectMouseClick()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    // ���������ײ����ǰ�����л�������Χ����ʾ״̬
                    if (hit.collider.gameObject == enemyAgent.gameObject)
                    {
                        ShowRangeView();
                    }
                }
            }
        }

        private void ShowRangeView()
        {
            print("ShowRangeView");
            rangeViewPrefab.SetActive(!rangeViewPrefab.activeSelf);
        }
        
        
    }
}