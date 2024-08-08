using _3DlevelEditor_GYS;
using Gameplay;
using Gameplay.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class BlockManager : MonoBehaviour
    {
        public List<StartPoint> startPoints = new List<StartPoint>();

        public static BlockManager instance;


        public Dictionary<SoliderAgent, List<GridCell>> headSoliderBlocks = new Dictionary<SoliderAgent, List<GridCell>>();
        public Dictionary<StartPoint, List<GridCell>> startPointBlocks = new Dictionary<StartPoint, List<GridCell>>();

        public Dictionary<GridCell, int> canPlaceBlocks = new Dictionary<GridCell, int>();

        private void Awake()
        {
            if (instance != null)
            {
                Debug.LogError("More than one BlockManager in scene!");
                return;
            }
            instance = this;
        }

        private void Start()
        {
            OnInit();
        }

        private void OnInit()
        {
            foreach (StartPoint startPoint in startPoints)
            {
                startPoint.OnInit();
            }

            CheckAllStartPoint();
            //���headSolider
            foreach (var cellList in headSoliderBlocks.Values)
            {
                foreach (GridCell cell in cellList)
                {
                    if (canPlaceBlocks.ContainsKey(cell))
                    {
                        canPlaceBlocks[cell]++;
                        Debug.Log("HeadSolider has key:" + cell.name + " " + canPlaceBlocks[cell]);
                    }
                    else
                    {
                        canPlaceBlocks.Add(cell, 1);
                        Debug.Log("HeadSolider no key:" + canPlaceBlocks[cell]);
                    }
                }
            }
            CheckCanPlace();
        }

        public void CheckAllStartPoint()
        {
            foreach (var cellList in startPointBlocks.Values)
            {
                foreach (GridCell cell in cellList)
                {
                    if (canPlaceBlocks.ContainsKey(cell))
                    {
                        canPlaceBlocks[cell]++;
                        Debug.Log("Start Point has key:" + canPlaceBlocks[cell]);
                    }
                    else
                    {
                        canPlaceBlocks.Add(cell, 1);
                        Debug.Log("Start Point no key:" + canPlaceBlocks[cell]);
                    }
                }
            }
        }

        public void CheckStartPoint()
        {
            foreach (var cellList in startPointBlocks.Values)
            {
                foreach (GridCell cell in cellList)
                {
                    if (canPlaceBlocks.ContainsKey(cell))
                    {
                        ++canPlaceBlocks[cell];
                        Debug.Log("Start Point has key:" + canPlaceBlocks[cell]);
                    }
                    else
                    {
                        canPlaceBlocks.Add(cell, 1);
                        Debug.Log("Start Point no key:" + canPlaceBlocks[cell]);
                    }
                }
            }
        }

        public void CheckHeadSolider(GridCell cell)
        {
            if (canPlaceBlocks.ContainsKey(cell))
            {
                canPlaceBlocks[cell]++;
                Debug.Log("HeadSolider has key:" + cell.name + " " + canPlaceBlocks[cell]);
            }
            else
            {
                canPlaceBlocks.Add(cell, 1);
                Debug.Log("HeadSolider no key:" + canPlaceBlocks[cell]);
            }
        }

        public void UpdateCell(SoliderAgent solider, GridCell cell)
        {
            if (headSoliderBlocks.ContainsKey(solider))
            {
                if (!headSoliderBlocks[solider].Contains(cell))
                {
                    headSoliderBlocks[solider].Add(cell);
                    Debug.Log(cell.name);
                }
            }
            else
            {
                List<GridCell> cells = new List<GridCell> { cell };
                headSoliderBlocks.Add(solider, cells);
                Debug.Log(cell.name);
            }
        }

        public void OnHeadSoliderDestory(SoliderAgent solider)
        {
            if (headSoliderBlocks.ContainsKey(solider))
            {
                foreach (var cell in headSoliderBlocks[solider])
                {
                    if (canPlaceBlocks.ContainsKey(cell))
                    {
                        canPlaceBlocks[cell]--;
                        Debug.Log("HeadSolider has key:" + cell.name + " " + canPlaceBlocks[cell]);
                        if (canPlaceBlocks[cell] <= 0)
                        {
                            cell.canPlace = false;
                            cell.OnCanPlaceChange(false);
                            canPlaceBlocks.Remove(cell);
                        }
                    }
                }
            }

            headSoliderBlocks.Remove(solider);
        }

        public void CheckCanPlace()
        {
            foreach (var cell in canPlaceBlocks.Keys)
            {
                if (canPlaceBlocks[cell] == 0)
                {
                    cell.canPlace = false;
                    cell.OnCanPlaceChange(false);
                    canPlaceBlocks.Remove(cell);
                }
                else
                {
                    if (cell.canPlace == false)
                    {
                        cell.canPlace = true;
                        cell.OnCanPlaceChange(true);
                    }
                }
            }
        }
    }

}