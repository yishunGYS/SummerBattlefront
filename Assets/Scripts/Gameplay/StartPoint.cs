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

        private GridCell cell;

        public List<GridCell> spawnBlocks = new List<GridCell>();

        public SerializableDictionary<StartPoint, List<GridCell>> previousCamps;

        private void Awake()
        {
            rend = GetComponent<Renderer>();
            cell = GetComponent<GridCell>();
            startColor = rend.material.color;
        }

        void Start()
        {
            BlockManager.instance.startPoints.Add(this);
        }

        public void OnInit()
        {

            foreach (GridCell cell in cell.nextCells)
            {
                spawnBlocks.Add(cell);
            }

            if (!BlockManager.instance.startPointBlocks.ContainsKey(this))
                BlockManager.instance.startPointBlocks.Add(this, spawnBlocks);
        }

        public void SetLastCampActive()
        {
            foreach (var startPoint in previousCamps.Keys)
            {
                BlockManager.instance.startPointBlocks.Add(startPoint, previousCamps[startPoint]);
            }
            BlockManager.instance.CheckAllStartPoint();
            BlockManager.instance.CheckCanPlace();
        }

    }
}