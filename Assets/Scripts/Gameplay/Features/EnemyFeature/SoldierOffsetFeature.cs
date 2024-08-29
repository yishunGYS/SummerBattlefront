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

        public void ApplyOffsetToSoldiers(HashSet<SoliderAgent> soldiers, Vector3 centerPosition)
        {
            if (soldiers == null || soldiers.Count == 0)
            {
                return;
            }

            int index = 0;

            foreach (var soldier in soldiers)
            {
                if (soldier.GetComponent<TraverserFeature>())
                {
                    continue;
                }
                float angle = 360f / soldiers.Count * index;
                Vector3 offset = new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), 0, Mathf.Cos(Mathf.Deg2Rad * angle)) * offsetDistance;

                soldier.transform.DOMove(centerPosition + offset, duration);

                index++;
            }
        }
    }
}
