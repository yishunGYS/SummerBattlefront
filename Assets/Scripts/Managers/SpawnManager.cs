using Gameplay.Enemy;
using Gameplay.Player;
using System.Collections;
using System.Collections.Generic;
using Systems;
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
                var id = selectedCharacter.soliderId;
                var tempSoliderModel = DataManager.Instance.GetSoliderDataById(id);
                if (tempSoliderModel.spawnNum <= 1)
                {
                    SpawnSingle(tempSoliderModel.cost);
                }
                else
                {
                    var tempSolider = selectedCharacter;
                    StartCoroutine(SpawnMultiple(tempSolider, tempSoliderModel.spawnNum, tempSoliderModel.cost));
                }
            }
            else
            {
                Debug.LogError("No character selected to spawn!");
            }
        }

        private void SpawnSingle(int cost)
        {
            if (PlayerStats.Money < cost)
            {
                Debug.Log("资源不够!");
                return;
            }

            SoliderAgent spawnedCharacter = Instantiate(selectedCharacter, spawnPoint.position, spawnPoint.rotation);
            if (SoliderContainer != null)
            {
                spawnedCharacter.transform.SetParent(SoliderContainer.transform);
                spawnedCharacter.OnInit();
            }

            PlayerStats.Money -= cost;
            // 设置路径编号
            spawnedCharacter.soliderLogic.SetPath(pathNum);
        }

        private IEnumerator SpawnMultiple(SoliderAgent soliderAgent , int spawnNum, int cost)
        {
            for (int i = 0; i < spawnNum; i++)
            {
                if (PlayerStats.Money < cost)
                {
                    Debug.Log("资源不够!");
                    yield break;
                }

                SoliderAgent spawnedCharacter = Instantiate(soliderAgent, spawnPoint.position, spawnPoint.rotation);
                if (SoliderContainer != null)
                {
                    spawnedCharacter.transform.SetParent(SoliderContainer.transform);
                    spawnedCharacter.OnInit();
                }

                PlayerStats.Money -= cost;
                // 设置路径编号
                spawnedCharacter.soliderLogic.SetPath(pathNum);

                yield return new WaitForSeconds(0.5f); // 延时0.5秒
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

        public SoliderAgent GetSolider()
        {
            return selectedCharacter;
        }
    }
}
