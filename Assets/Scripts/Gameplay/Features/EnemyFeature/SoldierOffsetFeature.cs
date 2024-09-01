using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Gameplay.Player;
using _3DlevelEditor_GYS;
using Gameplay.Features;

namespace Gameplay.Enemy
{
    public class SoldierOffsetFeature : MonoBehaviour
    {
        public float offsetDistance;
        public float duration;

        private EnemyAgent enemyAgent;
        private EnemyLogicBase enemyLogicBase;

        public GridCell currentCell;

        private void Start()
        {
            enemyAgent = GetComponent<EnemyAgent>();
            enemyLogicBase = enemyAgent.enemyLogic;

            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 10f))
            {
                currentCell = hit.collider.GetComponent<GridCell>();
            }
        }

        private void Update()
        {
            if (enemyLogicBase.blockSoilders.Count > 0 && currentCell != null)
            {
                Vector3 centerposition = currentCell.previousCells[0].transform.position + Vector3.up;
                ApplyOffsetToSoldiers(enemyLogicBase.blockSoilders, centerposition);
            }
        }

        public void ApplyOffsetToSoldiers(List<SoliderAgent> soldiers, Vector3 centerPosition)
        {
            if (soldiers == null || soldiers.Count == 0)
            {
                return;
            }

            int soldierCount = soldiers.Count;
            float maxRadius = offsetDistance;
            Vector3 currentPosition = this.transform.position;

            // 定义四个起始位置：上、下、左、右
            List<Vector3> startPositions = new List<Vector3>
            {
                centerPosition + new Vector3(0, 0, maxRadius),   // 上
                centerPosition + new Vector3(0, 0, -maxRadius),  // 下
                centerPosition + new Vector3(maxRadius, 0, 0),   // 右
                centerPosition + new Vector3(-maxRadius, 0, 0)   // 左
            };

            // 根据与挂载物体的位置距离对起始位置进行排序
            startPositions.Sort((a, b) =>
                Vector3.Distance(a, currentPosition).CompareTo(Vector3.Distance(b, currentPosition))
            );

            if (soldierCount == 1)
            {
                soldiers[0].transform.DOMove(startPositions[0], duration);
                return;
            }

            List<Vector3> positionsOnCircle = new List<Vector3>();
            for (int i = 0; i < soldierCount; i++)
            {
                float angle = 360f / soldierCount * i;
                Vector3 positionOnCircle = new Vector3(
                    Mathf.Sin(Mathf.Deg2Rad * angle),
                    0,
                    Mathf.Cos(Mathf.Deg2Rad * angle)
                ) * maxRadius;
                positionsOnCircle.Add(centerPosition + positionOnCircle);
            }

            positionsOnCircle.Sort((a, b) =>
                Vector3.Distance(a, currentPosition).CompareTo(Vector3.Distance(b, currentPosition))
            );

            for (int index = 0; index < soldierCount; index++)
            {
                var soldier = soldiers[index];

                if (soldier.GetComponent<TraverserFeature>())
                {
                    continue;
                }

                Vector3 targetPosition = positionsOnCircle[index];
                soldier.transform.DOMove(targetPosition, duration);
            }
        }


    }
}
