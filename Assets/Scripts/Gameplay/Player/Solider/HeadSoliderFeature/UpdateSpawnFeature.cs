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
                        BlockManager.instance.UpdateCell(agent, currentCell);//放入字典
                        BlockManager.instance.CheckHeadSolider(currentCell);
                        BlockManager.instance.CheckAllStartPoint();
                        BlockManager.instance.CheckCanPlace();
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
    }
}
