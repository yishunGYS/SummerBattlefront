using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using LevelEditor;
#if UNITY_EDITOR
namespace _3DlevelEditor_GYS
{
    public class LevelEditorWindow : EditorWindow
    {
        private BlockData blockData;
        private int selectedBlockIndex = 0;
        private Vector2Int gridSize = new Vector2Int(100, 100);
        private float cellSize = 1f;
        private Vector3 gridOffset = Vector3.zero;
        private bool isGenerating = false;
        private bool isSelectingPrevious = false;
        private bool isSelectingNext = false;
        private GameObject container;
        private GridCell selectedGridCell;
        private GridCell previousSelectedGridCell;
        private List<GridCell> selectableCells = new List<GridCell>();

        

        [MenuItem("Window/Level Editor")]
        public static void ShowWindow()
        {
            GetWindow<LevelEditorWindow>("Level Editor");
        }

        private void OnGUI()
        {
            GUILayout.Label("Grid Settings", EditorStyles.boldLabel);
            gridSize = EditorGUILayout.Vector2IntField("Grid Size", gridSize);
            cellSize = EditorGUILayout.FloatField("Cell Size", cellSize);
            gridOffset = EditorGUILayout.Vector3Field("Grid Offset", gridOffset);

            GUILayout.Label("Block Settings", EditorStyles.boldLabel);
            blockData = (BlockData)EditorGUILayout.ObjectField("Block Data", blockData, typeof(BlockData), false);

            if (blockData != null && blockData.blockPrefabs.Count > 0)
            {
                selectedBlockIndex = EditorGUILayout.Popup("Select Block", selectedBlockIndex, GetBlockNames(blockData.blockPrefabs));
                GUILayout.Label("Current Selected Block", EditorStyles.boldLabel);
                GUILayout.Label(blockData.blockPrefabs[selectedBlockIndex].name);
                GUILayout.Box(AssetPreview.GetAssetPreview(blockData.blockPrefabs[selectedBlockIndex]), GUILayout.Width(100), GUILayout.Height(100));
            }

            if (GUILayout.Button(isGenerating ? "Stop Generating" : "Start Generating"))
            {
                isGenerating = !isGenerating;
                isSelectingPrevious = false;
                isSelectingNext = false;

                if (isGenerating && container == null)
                {
                    CreateContainer();
                }
                SetShowConnections(false);
            }

            if (GUILayout.Button(isSelectingPrevious ? "Stop Selecting Previous" : "Select Previous"))
            {
                isSelectingPrevious = !isSelectingPrevious;
                isGenerating = false;
                isSelectingNext = false;
                SetShowConnections(false);
                selectedGridCell = null;
                previousSelectedGridCell = null;
                selectableCells.Clear();
            }

            if (GUILayout.Button(isSelectingNext ? "Stop Selecting Next" : "Select Next"))
            {
                isSelectingNext = !isSelectingNext;
                isGenerating = false;
                isSelectingPrevious = false;
                SetShowConnections(false);
                selectedGridCell = null;
                previousSelectedGridCell = null;
                selectableCells.Clear();
            }

            GUILayout.Label("Instructions", EditorStyles.boldLabel);
            GUILayout.Label("Left Click: Place Block", EditorStyles.label);
            GUILayout.Label("Right Click: Remove Block", EditorStyles.label);
            if (isSelectingPrevious)
            {
                GUILayout.Label("Click a block to set it as previous.", EditorStyles.label);
                if (selectedGridCell != null)
                {
                    GUILayout.Label("Currently selecting previous for: " + selectedGridCell.name, EditorStyles.label);
                }
            }
            else if (isSelectingNext)
            {
                GUILayout.Label("Click a block to set it as next.", EditorStyles.label);
                if (selectedGridCell != null)
                {
                    GUILayout.Label("Currently selecting next for: " + selectedGridCell.name, EditorStyles.label);
                }
            }
        }

        private void OnEnable()
        {
            SceneView.duringSceneGui += OnSceneGUI;
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
        }

        private void OnSceneGUI(SceneView sceneView)
        {
            if (!isGenerating && !isSelectingPrevious && !isSelectingNext) return;

            Handles.BeginGUI();
            GUILayout.BeginArea(new Rect(10, 10, 200, 50), "Level Editor", GUI.skin.window);
            GUILayout.Label("Left Click: Place Block");
            GUILayout.Label("Right Click: Remove Block");
            if (isSelectingPrevious)
            {
                GUILayout.Label("Click a block to set it as previous.");
            }
            else if (isSelectingNext)
            {
                GUILayout.Label("Click a block to set it as next.");
            }
            GUILayout.EndArea();
            Handles.EndGUI();

            Event e = Event.current;
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

            if ((e.type == EventType.MouseDown && (e.button == 0 || e.button == 1)))
            {
                Vector2 mousePosition = e.mousePosition;
                Ray ray = HandleUtility.GUIPointToWorldRay(mousePosition);
                Plane plane = new Plane(Vector3.up, gridOffset);

                if (plane.Raycast(ray, out float distance))
                {
                    Vector3 worldPosition = ray.GetPoint(distance);
                    int x = Mathf.FloorToInt((worldPosition.x - gridOffset.x) / cellSize);
                    int y = Mathf.FloorToInt((worldPosition.z - gridOffset.z) / cellSize);

                    if (e.button == 0)
                    {
                        if (isGenerating && blockData != null && selectedBlockIndex < blockData.blockPrefabs.Count)
                        {
                            if (!IsBlockPresent(worldPosition))
                            {
                                PlaceBlock(x, y);
                            }
                        }
                        else if (isSelectingPrevious || isSelectingNext)
                        {
                            SelectBlock(worldPosition);
                        }
                    }
                    else if (e.button == 1)
                    {
                        if (isGenerating)
                        {
                            RemoveBlock(x, y);
                        }
                        else if (isSelectingPrevious || isSelectingNext)
                        {
                            DeselectOrCancelSelection(worldPosition);
                        }
                    }

                    e.Use();
                }
            }

            DrawGrid();
            HighlightSelectableCells();
        }

        private void DrawGrid()
        {
            for (int x = 0; x <= gridSize.x; x++)
            {
                Vector3 start = gridOffset + new Vector3(x * cellSize, 0, 0);
                Vector3 end = gridOffset + new Vector3(x * cellSize, 0, gridSize.y * cellSize);
                Handles.DrawLine(start, end);
            }

            for (int y = 0; y <= gridSize.y; y++)
            {
                Vector3 start = gridOffset + new Vector3(0, 0, y * cellSize);
                Vector3 end = gridOffset + new Vector3(gridSize.x * cellSize, 0, y * cellSize);
                Handles.DrawLine(start, end);
            }
        }

        private void PlaceBlock(int x, int y)
        {
            var position = gridOffset + new Vector3(x * cellSize + cellSize / 2, 0, y * cellSize + cellSize / 2);
            GameObject blockObject = Instantiate(blockData.blockPrefabs[selectedBlockIndex], position, Quaternion.identity);
            blockObject.name = blockData.blockPrefabs[selectedBlockIndex].name + $" ({x},{y})";
            blockObject.transform.parent = container.transform;
        }

        private void RemoveBlock(int x, int y)
        {
            var position = gridOffset + new Vector3(x * cellSize + cellSize / 2, 0, y * cellSize + cellSize / 2);
            Ray ray = new Ray(position + Vector3.up * 10, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                if (hit.collider.GetComponent<GridCell>() != null)
                {
                    DestroyImmediate(hit.collider.gameObject);
                }
            }
        }

        private bool IsBlockPresent(Vector3 worldPosition)
        {
            Ray ray = new Ray(worldPosition + Vector3.up * 10, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                if (hit.collider.GetComponent<GridCell>() != null)
                {
                    return true;
                }
            }
            return false;
        }

        private void SelectBlock(Vector3 worldPosition)
        {
            Ray ray = new Ray(worldPosition + Vector3.up * 10, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                GridCell hitCell = hit.collider.GetComponent<GridCell>();
                if (hitCell != null)
                {
                    if (isSelectingPrevious || isSelectingNext)
                    {
                        if (selectedGridCell == null)
                        {
                            if (previousSelectedGridCell != null)
                            {
                                previousSelectedGridCell.showConnections = false;
                            }
                            selectedGridCell = hitCell;
                            selectedGridCell.showConnections = true;
                            previousSelectedGridCell = selectedGridCell;
                            HighlightAdjacentCells(selectedGridCell);
                        }
                        else
                        {
                            if (selectableCells.Contains(hitCell))
                            {
                                if (isSelectingPrevious && !selectedGridCell.previousCells.Contains(hitCell) && !selectedGridCell.nextCells.Contains(hitCell))
                                {
                                    selectedGridCell.previousCells.Add(hitCell);
                                }
                                else if (isSelectingNext && !selectedGridCell.nextCells.Contains(hitCell) && !selectedGridCell.previousCells.Contains(hitCell))
                                {
                                    selectedGridCell.nextCells.Add(hitCell);
                                }
                                selectableCells.Clear();
                                selectedGridCell.showConnections = true;
                                previousSelectedGridCell = selectedGridCell;
                                selectedGridCell = null;
                            }
                        }
                    }
                }
            }
        }

        private void DeselectOrCancelSelection(Vector3 worldPosition)
        {
            Ray ray = new Ray(worldPosition + Vector3.up * 10, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                GridCell hitCell = hit.collider.GetComponent<GridCell>();
                if (hitCell != null)
                {
                    if (isSelectingPrevious || isSelectingNext)
                    {
                        if (selectedGridCell == hitCell)
                        {
                            selectedGridCell.showConnections = false;
                            selectedGridCell = null;
                            previousSelectedGridCell = null;
                            selectableCells.Clear();
                        }
                        else
                        {
                            if (isSelectingPrevious)
                            {
                                selectedGridCell.previousCells.Remove(hitCell);
                            }
                            else if (isSelectingNext)
                            {
                                selectedGridCell.nextCells.Remove(hitCell);
                            }
                            HighlightAdjacentCells(selectedGridCell);
                        }
                    }
                }
            }
        }

        private void HighlightAdjacentCells(GridCell cell)
        {
            selectableCells.Clear();
            Vector3[] directions = new Vector3[]
            {
                Vector3.forward * cellSize,
                Vector3.back * cellSize,
                Vector3.left * cellSize,
                Vector3.right * cellSize
            };

            foreach (var direction in directions)
            {
                Ray ray = new Ray(cell.transform.position + direction + Vector3.up * 10, Vector3.down);
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
                {
                    GridCell adjacentCell = hit.collider.GetComponent<GridCell>();
                    if (adjacentCell != null)
                    {
                        selectableCells.Add(adjacentCell);
                    }
                }
            }
        }

        private void HighlightSelectableCells()
        {
            Handles.color = Color.yellow;
            foreach (var cell in selectableCells)
            {
                Handles.DrawWireCube(cell.transform.position, Vector3.one * cellSize);
            }
        }

        private void CreateContainer()
        {
            container = new GameObject("Level Container");
        }

        private void SetShowConnections(bool show)
        {
            if (container == null) return;

            foreach (Transform child in container.transform)
            {
                GridCell cell = child.GetComponent<GridCell>();
                if (cell != null)
                {
                    cell.showConnections = show;
                }
            }
        }

        private string[] GetBlockNames(List<GameObject> blockPrefabs)
        {
            string[] names = new string[blockPrefabs.Count];
            for (int i = 0; i < blockPrefabs.Count; i++)
            {
                names[i] = blockPrefabs[i] != null ? blockPrefabs[i].name : "None";
            }
            return names;
        }
    }
}
#endif