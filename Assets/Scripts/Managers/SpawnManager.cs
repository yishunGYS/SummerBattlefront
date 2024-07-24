using Gameplay.Player;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class SpawnManager : MonoBehaviour
    {
        public static SpawnManager instance;

        public List<GameObject> characters = new List<GameObject>();
        public Transform spawnPoint;
        public GameObject SoliderContainer;

        // 当前选中的角色
        public SoliderAgent selectedCharacter;
        public int pathNum;

        private void Awake()
        {
            if (instance != null)
            {
                Debug.LogError("More than one SpawnManager in scene!");
                return;
            }
            instance = this;
        }

        public void ChangeSpawnCharacter(SoliderAgent chara)
        {
            selectedCharacter = chara;
        }

        public void SpawnCharacter()
        {
            if (selectedCharacter != null)
            {
                SoliderAgent spawnedCharacter = Instantiate(selectedCharacter, spawnPoint.position, spawnPoint.rotation);
                if (SoliderContainer != null)
                {
                    spawnedCharacter.transform.SetParent(SoliderContainer.transform);
                    spawnedCharacter.OnInit();
                }

                // 设置路径编号
                EnemyMovement enemyMovement = spawnedCharacter.GetComponent<EnemyMovement>();
                if (enemyMovement != null)
                {
                    enemyMovement.SetPath(pathNum);
                }
            }
            else
            {
                Debug.LogError("No character selected to spawn!");
            }
        }

        public void ChangeSpawnPoint(Transform transform)
        {
            spawnPoint = transform;
        }

        public void SetPathNum(int num)
        {
            pathNum = num;
        }
    }
}
