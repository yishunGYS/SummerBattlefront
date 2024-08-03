using _3DlevelEditor_GYS;
using Gameplay.Player;
using Managers;
using System.Collections.Generic;
using Systems;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Gameplay
{
    public class StartPoint : MonoBehaviour
    {
        private Color startColor;
        private Color notEnoughMoneyColor;

        private Renderer rend;

        public Color hoverColor;


        public List<GridCell> spawnBlocks = new List<GridCell>();

        void Start()
        {
            OnInit();
        }

        private void OnInit()
        {
            rend = GetComponent<Renderer>();
            startColor = rend.material.color;

            var blocks = transform.GetComponentsInChildren<GridCell>();
            foreach (GridCell cell in blocks)
            {
                spawnBlocks.Add(cell);
            }

            BlockManager.instance.startPointBlocks.Add(this, spawnBlocks);
        }

        private void SpawnCharacter(SoliderAgent chara)
        {
            SpawnManager.instance.ChangeSpawnPoint(this.transform);
            //SpawnManager.instance.SetPathNum(pathNum);
            //SpawnManager.instance.SpawnCharacter();
        }
    }
}
