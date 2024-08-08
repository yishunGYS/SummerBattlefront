using _3DlevelEditor_GYS;
using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

namespace Gameplay.Player
{
    public class UpdateSpawnFeature : MonoBehaviour
    {
        SoliderAgent agent;
        private GridCell lastDetectedCell;

        private GridCell initBlock;

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
                // ���Դ�������ײ�Ķ����л�ȡָ�����
                GridCell currentCell = hit.collider.GetComponent<GridCell>();

                if (currentCell != null)
                {

                    // ������⵽�����巢���仯ʱ�ŵ��� UpdateCell
                    if (currentCell != lastDetectedCell)
                    {
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
