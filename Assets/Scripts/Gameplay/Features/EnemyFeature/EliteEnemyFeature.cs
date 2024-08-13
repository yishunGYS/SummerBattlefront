using System.Collections.Generic;
using _3DlevelEditor_GYS;
using Gameplay.Enemy;
using Managers;
using UnityEngine;

namespace Gameplay.Features.EnemyFeature
{
    [RequireComponent(typeof(EnemyAgent))]
    public class EliteEnemyFeature : MonoBehaviour
    {
        EnemyAgent agent;
        private GridCell lastDetectedCell;

        public GameObject campPrefab;

        //若精英怪不在岔路口的话，currentBlock需要策划手动拖~
        public GridCell currentBlock;

        public StartPoint lastCamp;

        private void Awake()
        {
            agent = GetComponent<EnemyAgent>();

            campPrefab = Resources.Load<GameObject>("BlockPrefab/SpawnPoint");
        }

        private void Start()
        {
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 10f))
            {
                GridCell currentCell = hit.collider.GetComponent<GridCell>();

                currentBlock = currentCell;
            }
        }
        
        public void SpawnCamp(GridCell cell)
        {
            List<GridCell> newPreviousCell = cell.previousCells;
            List<GridCell> newNextCell = cell.nextCells;

            foreach (GridCell cell_p in cell.previousCells)
            {
                cell_p.nextCells.Remove(cell);
            }

            foreach (GridCell cell_n in cell.nextCells)
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

            foreach (GridCell nextcell in gridCell.nextCells)
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
            SpawnCamp(currentBlock);
            setLastCampUnActive();
            BlockManager.instance.CheckAllStartPoint();
            BlockManager.instance.CheckCanPlace();
        }
    }
}

