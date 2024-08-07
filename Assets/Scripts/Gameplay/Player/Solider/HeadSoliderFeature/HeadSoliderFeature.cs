using _3DlevelEditor_GYS;
using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

namespace Gameplay.Player
{
    public class HeadSoliderFeature : MonoBehaviour
    {
        SoliderAgent agent;
        private GridCell lastDetectedCell;

        public GameObject campPrefab;
        public GameObject cubePrefab;

        public StartPoint lastCamp;
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

                lastCamp = initBlock.previousCells[0].gameObject.GetComponent<StartPoint>();
            }
        }

        public void UpdateCanPlace()
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

        public void CheckIsCrossRoad()
        {
            if (agent.soliderLogic.nextBlock.Count > 1)
            {
                destoryLastCamp();
                SpawnCamp(agent.soliderLogic.currentBlock);
                BlockManager.instance.CheckAllStartPoint();
                BlockManager.instance.CheckCanPlace();
            }
        }

        public void SpawnCamp(GridCell cell)
        {
            List<GridCell> newPreviousCell = cell.previousCells;
            List<GridCell> newNextCell = cell.nextCells;

            foreach(GridCell cell_p in cell.previousCells)
            {
                cell_p.nextCells.Remove(cell);
            }

            foreach(GridCell cell_n in cell.nextCells)
            {
                cell_n.previousCells.Remove(cell);
            }

            var position = cell.gameObject.transform.position;
            GameObject.Destroy(cell.gameObject);
            var instance = Instantiate(campPrefab, position, Quaternion.identity);
            GridCell gridCell = instance.GetComponent<GridCell>();
            gridCell.previousCells = newPreviousCell;
            gridCell.nextCells = newNextCell;

            StartPoint startPoint = instance.GetComponent<StartPoint>();
            BlockManager.instance.startPointBlocks.Add(startPoint, newNextCell);

            foreach (GridCell previouscell in gridCell.previousCells)
            {
                previouscell.nextCells.Add(gridCell);
            }

            foreach(GridCell nextcell in gridCell.nextCells)
            {
                nextcell.previousCells.Add(gridCell);
            }
        }

        public void destoryLastCamp()
        {
            var position = lastCamp.gameObject.transform.position;
            var lastCampCell = lastCamp.gameObject.GetComponent<GridCell>();

            //���BlockManager�е�����
            foreach(var cell in lastCampCell.nextCells)
            {
                if(BlockManager.instance.canPlaceBlocks.ContainsKey(cell))
                {
                    BlockManager.instance.canPlaceBlocks.Remove(cell);
                    cell.canPlace = false;
                    cell.OnCanPlaceChange(false);
                }
            }

            BlockManager.instance.startPointBlocks.Remove(lastCamp);

            GameObject.Destroy(lastCamp.gameObject);

            Instantiate(cubePrefab, position, Quaternion.identity);

        }
    }
}
