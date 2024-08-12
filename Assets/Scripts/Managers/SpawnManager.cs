using System;
using _3DlevelEditor_GYS;
using Gameplay;
using Gameplay.Enemy;
using Gameplay.Player;
using System.Collections;
using System.Collections.Generic;
using Systems;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Managers
{
    public class SpawnManager : MonoBehaviour
    {
        public static SpawnManager instance;

        public List<GameObject> characters = new List<GameObject>();
        public GameObject SoliderContainer;

        // ��ǰѡ�еĽ�ɫ
        public SoliderAgent selectedCharacter;

        //�������/����ؿ�
        public LayerMask groundLayer;
        private GameObject hoveredObject; // ��ǰ����������
        
        
        private void Awake()
        {
            if (instance != null)
            {
                Debug.LogError("More than one SpawnManager in scene!");
                return;
            }
            instance = this;
        }


        private void Update()
        {
            DetectMouseClick();
            DetectMouseHover();
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
                Debug.Log("��Դ����!");
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
            // ����·�����
        }

        private IEnumerator SpawnMultiple(GridCell block, SoliderAgent soliderAgent , int spawnNum, int cost)
        {
            for (int i = 0; i < spawnNum; i++)
            {
                if (PlayerStats.Money < cost)
                {
                    Debug.Log("��Դ����!");
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

                yield return new WaitForSeconds(0.5f); // ��ʱ0.5��
            }
        }
        

        public SoliderAgent GetSolider()
        {
            return selectedCharacter;
        }


        private void DetectMouseClick()
        {            //���������ؿ�
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
                {
                    // �ڵؿ�������ʿ��
                    GameObject hitObject = hit.collider.gameObject;
                    GridCell block = hitObject.GetComponent<GridCell>();
                    if ( block.canPlace)
                    {
                        SpawnCharacter(block);
                    }
                }
            }
        }



        private void DetectMouseHover()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
            {
                GameObject hitObject = hit.collider.gameObject;
                GridCell block = hitObject.GetComponent<GridCell>();

                if (block != null)
                {
                    if (hoveredObject != hitObject)
                    {
                        // ��⵽�µ���������
                        if (hoveredObject != null)
                        {
                            // ֮ǰ����������ִ���뿪�������߼�
                            OnMouseExitBlock(hoveredObject.GetComponent<GridCell>());
                        }
                        // ��ǰ����������
                        hoveredObject = hitObject;
                        OnMouseEnterBlock(block);
                    }
                }
            }
            else
            {
                if (hoveredObject != null)
                {
                    // ����뿪�����ĵؿ�
                    OnMouseExitBlock(hoveredObject.GetComponent<GridCell>());
                    hoveredObject = null;
                }
            }
        }
        
        
        void OnMouseEnterBlock(GridCell block)
        {
            // ��������ڵؿ���߼�
            if (block.canPlace)
            {
                //������Ϸ�еĵ����͸ UI
                if (EventSystem.current.IsPointerOverGameObject())
                    return;

                block.rend.material.color = block.hoverColor;
            }
        }

        void OnMouseExitBlock(GridCell block)
        {
            // ����뿪�ؿ���߼�
            if (block.canPlace)
            {
                block.rend.material.color = block.canSpawnColor;
            }
        }
    }
}
