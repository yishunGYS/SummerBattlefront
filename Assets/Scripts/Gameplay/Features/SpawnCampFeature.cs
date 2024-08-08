using System.Collections.Generic;
using _3DlevelEditor_GYS;
using Gameplay.Player;
using Managers;
using UnityEngine;

namespace Gameplay.Features
{
    public class SpawnCampFeature : MonoBehaviour
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

        public void Update()
        {
            BlockManager.instance.CheckCanPlace();
            Vector3 origin = transform.position;
            Vector3 direction = Vector3.down;

            float rayLength = 10f;

            Debug.DrawRay(origin, direction * rayLength, Color.yellow);

            // 执行射线检测
            if (Physics.Raycast(origin, direction, out RaycastHit hit, rayLength))
            {
                // 尝试从射线碰撞的对象中获取指定组件
                GridCell currentCell = hit.collider.GetComponent<GridCell>();

                if (currentCell != null)
                {

                    // 仅当检测到的物体发生变化时才调用 UpdateCell
                    if (currentCell != lastDetectedCell)
                    {
                        lastDetectedCell = currentCell; // 更新最后检测到的物体
                    }
                }
                else
                {
                    lastDetectedCell = null; // 重置最后检测到的物体
                }
            }
            else
            {
                lastDetectedCell = null; // 重置最后检测到的物体
            }
        }

        public void CheckIsCrossRoad()
        {
            Debug.Log("CheckIsCrossRoad called");
            if (agent.soliderLogic.nextBlock.Count > 1)
            {
                Debug.Log("Crossroad detected");
                //destoryLastCamp();
                SpawnCamp(agent.soliderLogic.currentBlock);
                setLastCampUnActive();
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

            setCampDate(startPoint);

            foreach (GridCell previouscell in gridCell.previousCells)
            {
                previouscell.nextCells.Add(gridCell);
            }

            foreach(GridCell nextcell in gridCell.nextCells)
            {
                nextcell.previousCells.Add(gridCell);
            }
        }

        public void setLastCampUnActive()
        {
            var position = lastCamp.gameObject.transform.position;
            var lastCampCell = lastCamp.gameObject.GetComponent<GridCell>();

            //清除BlockManager中的数据
            foreach (var cell in lastCampCell.nextCells)
            {
                if (BlockManager.instance.canPlaceBlocks.ContainsKey(cell))
                {
                    BlockManager.instance.canPlaceBlocks.Remove(cell);
                    cell.canPlace = false;
                    cell.OnCanPlaceChange(false);
                }
            }

            BlockManager.instance.startPointBlocks.Remove(lastCamp);

        }

        public void setCampDate(StartPoint start)
        {
            start.previousCamps.Add(lastCamp, BlockManager.instance.startPointBlocks[lastCamp]);
        }

        private void OnDestroy()
        {
            Debug.Log("Crossroad detected");
            SpawnCamp(agent.soliderLogic.currentBlock);
            setLastCampUnActive();
            BlockManager.instance.CheckAllStartPoint();
            BlockManager.instance.CheckCanPlace();
        }
    }
}
