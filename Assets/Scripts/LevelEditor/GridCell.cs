using Managers;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _3DlevelEditor_GYS
{
    public enum BlockType
    {
        None,
        Path
    }

    public class GridCell : MonoBehaviour
    {
        private Renderer rend;

        public List<GridCell> previousCells = new List<GridCell>();
        public List<GridCell> nextCells = new List<GridCell>();
        public bool showConnections = false;
        public float lineWidth = 5f;

        [Header("×´Ì¬")]
        public bool canPlace = false;

        [Header("ÑÕÉ«")]
        public Color hoverColor = Color.red;
        public Color canSpawnColor = Color.yellow;
        private Color originalColor;

        public BlockType blockType;

        private void Start()
        {
            rend = gameObject.GetComponent<Renderer>();
            originalColor = rend.material.color;
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

        void OnMouseEnter()
        {
            if (blockType == BlockType.Path)
            {
                if (EventSystem.current.IsPointerOverGameObject())
                    return;

                rend.material.color = hoverColor;
            }
        }

        void OnMouseExit()
        {
            if (blockType == BlockType.Path)
            {
                rend.material.color = originalColor;
            }
        }

        private void OnMouseDown()
        {
            if (blockType == BlockType.Path && canPlace)
            {
                SpawnManager.instance.SpawnCharacter(this);
            }
        }

        public void SwitchState()
        {

        }
    }
}
