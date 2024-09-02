using _3DlevelEditor_GYS;
using Managers;
using System.Collections;
using System.Collections.Generic;
using Gameplay.Features.EnemyFeature;
using UnityEngine;
using static UnityEngine.UI.Image;

namespace Gameplay.Player
{
    public class UpdateSpawnFeature : MonoBehaviour
    {
        SoliderAgent agent;
        private GridCell lastDetectedCell;

        private GridCell initBlock;

        public StartPoint startPoint;

        private void Awake()
        {
            agent = GetComponent<SoliderAgent>();
        }

        private void Start()
        {
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 10f))
            {
                GridCell currentCell = hit.collider.GetComponent<GridCell>();
                initBlock = currentCell;
                startPoint = initBlock.previousCells[0].GetComponent<StartPoint>();
            }
        }

        public void Update()
        {

            BlockManager.instance.CheckCanPlace();
            Vector3 origin = transform.position;
            Vector3 direction = Vector3.down;

            float rayLength = 10f;

            Debug.DrawRay(origin, direction * rayLength, Color.yellow);

            // ִ�����߼��
            if (Physics.Raycast(origin, direction, out RaycastHit hit, rayLength))
            {
                if (hit.collider.GetComponent<UntargetableFeature>())
                {
                    return;
                }
                // ���Դ�������ײ�Ķ����л�ȡָ�����
                GridCell currentCell = hit.collider.GetComponent<GridCell>();

                if (currentCell != null)
                {
                    
                    // ������⵽�����巢���仯ʱ�ŵ��� UpdateCell
                    if (currentCell != lastDetectedCell)
                    {
                        Debug.Log($"HeadSolider reach{currentCell.name}");
                        BlockManager.instance.UpdateCell(agent, currentCell);//�����ֵ�
                        BlockManager.instance.CheckHeadSolider(currentCell);
                        BlockManager.instance.CheckAllStartPoint();
                        BlockManager.instance.CheckCanPlace();
                        lastDetectedCell = currentCell; // ��������⵽������
                    }
                }
                else
                {
                    lastDetectedCell = null; // ��������⵽������
                }
            }
            else
            {
                lastDetectedCell = null; // ��������⵽������
            }
        }
    }
}
