using System;
using Gameplay;
using Gameplay.Enemy;
using Gameplay.Player;
using System.Collections;
using System.Collections.Generic;
using Systems;
using UnityEngine;
using UnityEngine.EventSystems;
using Utilities;
using Gameplay.Item;
using _3DlevelEditor_GYS;
using Systems.Edu;

namespace Managers
{
    public class SpawnManager : Singleton<SpawnManager>
    {
        public GameObject SoliderContainer;

        // ��ǰѡ�еĽ�ɫ
        private SoliderAgent selectedCharacter;
        private int selectId;

        //�������/����ؿ�
        public LayerMask groundLayer;
        private GameObject hoveredObject; // ��ǰ����������

        public bool isLevelStarted = false;
        public bool canPlace = false;

        public void OnStart()
        {
            SoliderContainer = GameObject.Find("SoliderContainer");
            isLevelStarted = false;
        }

        private void Update()
        {
            DetectMouseClick();
            DetectMouseHover();
        }

        public void ChangeSelectSolider(int id)
        {
            var soliderModel = DataManager.Instance.GetSoliderDataById(id);
            selectedCharacter = Resources.Load<SoliderAgent>(soliderModel.scenePrefabPath);
        }

        private void SpawnCharacter(GridCell block)
        {
            if (selectedCharacter != null)
            {
                var id = selectedCharacter.soliderId;
                var tempSoliderModel = DataManager.Instance.GetSoliderDataById(id);
                SpawnSingle(block, tempSoliderModel.cost);
            }
            else
            {
                Debug.LogError("No character selected to spawn!");
            }
        }

        private void SpawnSingle(GridCell block, int cost)
        {
            if (EduSystem.Instance && EduSystem.Instance.isInEdu)
            {
                return;
            }
            
            if (!isLevelStarted)
            {
                return;
            }

            if (PlayerStats.Money < cost)
            {
                UIManager.Instance.OnShowTipPanel("Ŀǰ��Դ������",2000);
                return;
            }
            //PlayerStats.Instance.StartLevel();
            
            SoliderAgent spawnedCharacter = Instantiate(selectedCharacter, block.transform.position + Vector3.up,
                block.transform.rotation);
            if (SoliderContainer != null)
            {
                spawnedCharacter.transform.SetParent(SoliderContainer.transform);
                spawnedCharacter.OnInit();
                spawnedCharacter.soliderLogic.InitBlockData(block);
                spawnedCharacter.soliderLogic.InitBirthPointData(block);
                ItemManager.instance.RiseSoliderStats(spawnedCharacter);

                List<SoliderAgent> tempChilds = new List<SoliderAgent>();
                foreach (Transform child in spawnedCharacter.transform)
                { 
                    var childSoliderCmpt = child.GetComponent<SoliderAgent>();
                    if (childSoliderCmpt != null)
                    {
                        tempChilds.Add(childSoliderCmpt);
                    }
                }
                foreach (var solider in tempChilds)
                {
                    solider.transform.SetParent(SoliderContainer.transform);
                    solider.OnInit();
                    solider.soliderLogic.InitBlockData(block);
                    solider.soliderLogic.InitBirthPointData(block);
                    ItemManager.instance.RiseSoliderStats(spawnedCharacter);
                }
            }
            
            PlayerStats.Money -= cost;
        }
        

        public SoliderAgent GetSolider()
        {
            return selectedCharacter;
        }

        private void DetectMouseClick()
        {
            // ���������ؿ�
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
                {
                    // �ڵؿ�������ʿ��
                    GameObject hitObject = hit.collider.gameObject;
                    if (hitObject != null)
                    {
                        GridCell block = hitObject.GetComponent<GridCell>();
                        if (block != null)
                        {
                            if (block.canPlace)
                            {
                                SpawnCharacter(block);
                            }
                        }
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