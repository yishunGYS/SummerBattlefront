using Gameplay.Enemy;
using Managers;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.UI.Image;

namespace _3DlevelEditor_GYS
{
    public enum BlockType
    {
        None,
        Path,
        CanCamp
    }

    public class GridCell : MonoBehaviour
    {
        [HideInInspector]public Renderer rend;

        public List<GridCell> previousCells = new List<GridCell>();
        public List<GridCell> nextCells = new List<GridCell>();
        public bool showConnections = false;
        public float lineWidth = 5f;

        [Header("??")]
        public bool canPlace = false;

        [Header("???")]
        public Color hoverColor = Color.red;
        public Color canSpawnColor = Color.yellow;
        private Color originalColor;

        //public BlockType blockType;

        private void Awake()
        {
            rend = gameObject.GetComponent<Renderer>();
            originalColor = rend.material.color;

            //startPointPrefab = Resources.Load<GameObject>("BlockPrefab/SpawnPoint");
        }

        private void OnDrawGizmos()
        {
            if (showConnections)
            {
                Handles.color = Color.red;
                foreach (var previousCell in previousCells)
                {
                    if (previousCell != null)
                    {
                        DrawThickLine(transform.position, previousCell.transform.position, lineWidth, Handles.color);
                    }
                }

                Handles.color = Color.green;
                foreach (var nextCell in nextCells)
                {
                    if (nextCell != null)
                    {
                        DrawThickLine(transform.position, nextCell.transform.position, lineWidth, Handles.color);
                    }
                }
            }
        }

        private void DrawThickLine(Vector3 start, Vector3 end, float width, Color color)
        {
            Handles.color = color;
            Handles.DrawAAPolyLine(width, new Vector3[] { start, end });
        }
        

        public void OnCanPlaceChange(bool can)
        {
            if (can == true)
            {
                rend.material.color = canSpawnColor;
            }
            else
            {
                rend.material.color = originalColor;
            }
        }
    }
}
