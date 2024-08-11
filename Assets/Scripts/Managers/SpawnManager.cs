using _3DlevelEditor_GYS;
using Gameplay;
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
        public GameObject SoliderContainer;

        // 当前选中的角色
        public SoliderAgent selectedCharacter;

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

        public void SpawnCharacter(GridCell block)
        {
            if (selectedCharacter != null)
            {
                var id = selectedCharacter.soliderId;
                var tempSoliderModel = DataManager.Instance.GetSoliderDataById(id);
                if (tempSoliderModel.spawnNum <= 1)
                {
                    SpawnSingle(block, tempSoliderModel.cost);
                }
                else
                {
                    var tempSolider = selectedCharacter;
                    StartCoroutine(SpawnMultiple(block, tempSolider, tempSoliderModel.spawnNum, tempSoliderModel.cost));
                }
            }
            else
            {
                Debug.LogError("No character selected to spawn!");
            }
        }

        private void SpawnSingle(GridCell block, int cost)
        {
            if (PlayerStats.Money < cost)
            {
                Debug.Log("资源不够!");
                return;
            }

            SoliderAgent spawnedCharacter = Instantiate(selectedCharacter, block.transform.position + Vector3.up, block.transform.rotation);
            if (SoliderContainer != null)
            {
                spawnedCharacter.transform.SetParent(SoliderContainer.transform);
                spawnedCharacter.OnInit();
                spawnedCharacter.soliderLogic.InitBlockData(block);
                spawnedCharacter.soliderLogic.InitBirthPointData(block);
            }

            PlayerStats.Money -= cost;
            // 设置路径编号
        }

        private IEnumerator SpawnMultiple(GridCell block, SoliderAgent soliderAgent , int spawnNum, int cost)
        {
            for (int i = 0; i < spawnNum; i++)
            {
                if (PlayerStats.Money < cost)
                {
                    Debug.Log("资源不够!");
                    yield break;
                }

                SoliderAgent spawnedCharacter = Instantiate(selectedCharacter, block.transform.position + new Vector3(0f, block.transform.localScale.y, 0f), block.transform.rotation);
                if (SoliderContainer != null)
                {
                    spawnedCharacter.transform.SetParent(SoliderContainer.transform);
                    spawnedCharacter.OnInit();
                    spawnedCharacter.soliderLogic.InitBlockData(block);
                    spawnedCharacter.soliderLogic.InitBirthPointData(block);
                }

                PlayerStats.Money -= cost;
                // 设置路径编号
                //spawnedCharacter.soliderLogic.SetPath(pathNum);

                yield return new WaitForSeconds(0.5f); // 延时0.5秒
            }
        }

        public void ChangeSpawnPoint(Transform transform)
        {
            //spawnPoint = transform;
        }

        public void SetPathNum(int num)
        {
            //pathNum = num;
        }

        public SoliderAgent GetSolider()
        {
            return selectedCharacter;
        }
    }
}
