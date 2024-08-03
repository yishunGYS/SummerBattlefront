using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace _3DlevelEditor_GYS
{
    public class LevelEditorWindow : EditorWindow
    {
        private BlockData selectedBlock;
        private Vector2Int gridSize = new Vector2Int(10, 10);
        private float cellSize = 1f;
        private GridCell[,] grid;

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
            
            
            GUILayout.Label("Block Settings", EditorStyles.boldLabel);
            selectedBlock =
                (BlockData)EditorGUILayout.ObjectField("Selected Block", selectedBlock, typeof(BlockData), false);

            if (GUILayout.Button("Generate Grid"))
            {
                GenerateGrid();
            }

            if (GUILayout.Button("Save Level"))
            {
                SaveLevel();
            }

            GUILayout.Label("Instructions", EditorStyles.boldLabel);
            GUILayout.Label("Left Click: Place Block", EditorStyles.label);
            GUILayout.Label("Right Click: Remove Block", EditorStyles.label);
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
            Handles.BeginGUI();
            GUILayout.BeginArea(new Rect(10, 10, 200, 50), "Level Editor", GUI.skin.window);
            GUILayout.Label("Left Click: Place Block");
            GUILayout.Label("Right Click: Remove Block");
            GUILayout.EndArea();
            Handles.EndGUI();

            Event e = Event.current;
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

            if ((e.type == EventType.MouseDown && (e.button == 0 || e.button == 1)))
            {
                Vector2 mousePosition = e.mousePosition;

                Ray ray = HandleUtility.GUIPointToWorldRay(mousePosition);
                Plane plane = new Plane(Vector3.up, Vector3.zero); // 创建与世界坐标的 (0,0) 对齐的平面

                if (plane.Raycast(ray, out float distance))
                {
                    Vector3 worldPosition = ray.GetPoint(distance);
                    int x = Mathf.FloorToInt(worldPosition.x / cellSize);
                    int y = Mathf.FloorToInt(worldPosition.z / cellSize);

                    Debug.Log($"World Position: {worldPosition}"); // 打印世界坐标

                    if (e.button == 0 && selectedBlock != null)
                    {
                        PlaceBlock(x, y);
                    }
                    else if (e.button == 1)
                    {
                        RemoveBlock(x, y);
                    }

                    e.Use();
                }
            }

            DrawGrid();
        }

        private void DrawGrid()
        {
            for (int x = 0; x <= gridSize.x; x++)
            {
                Vector3 start = new Vector3(x * cellSize, 0, 0);
                Vector3 end = new Vector3(x * cellSize, 0, gridSize.y * cellSize);
                Handles.DrawLine(start, end);
            }

            for (int y = 0; y <= gridSize.y; y++)
            {
                Vector3 start = new Vector3(0, 0, y * cellSize);
                Vector3 end = new Vector3(gridSize.x * cellSize, 0, y * cellSize);
                Handles.DrawLine(start, end);
            }
        }

        private void GenerateGrid()
        {
            grid = new GridCell[gridSize.x, gridSize.y];
        }

        private void PlaceBlock(int x, int y)
        {
            Debug.Log("Placing block at (" + x + ", " + y);
            var gridOffset = cellSize / 2;
            if (x >= 0 && x < gridSize.x && y >= 0 && y < gridSize.y)
            {
                if (grid[x, y] == null)
                {
                    GameObject blockObject = Instantiate(selectedBlock.blockPrefab,
                        new Vector3(x * cellSize+gridOffset, 0, y * cellSize+gridOffset), Quaternion.identity);
                    grid[x, y] = blockObject.GetComponent<GridCell>();
                }
            }
        }

        private void RemoveBlock(int x, int y)
        {
            if (x >= 0 && x < gridSize.x && y >= 0 && y < gridSize.y)
            {
                if (grid[x, y] != null)
                {
                    DestroyImmediate(grid[x, y].gameObject);
                    grid[x, y] = null;
                }
            }
        }

        private void SaveLevel()
        {
            LevelData levelData = new LevelData();

            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    if (grid[x, y] != null)
                    {
                        CellData cellData = new CellData
                        {
                            position = new Vector2Int(x, y),
                            blockType = grid[x, y].gameObject.name
                        };
                        levelData.cells.Add(cellData);
                    }
                }
            }

            string json = JsonUtility.ToJson(levelData, true);
            System.IO.File.WriteAllText("Assets/LevelData.json", json);
        }
    }

    [System.Serializable]
    public class LevelData
    {
        public List<CellData> cells = new List<CellData>();
    }

    [System.Serializable]
    public class CellData
    {
        public Vector2Int position;
        public string blockType;
    }
}