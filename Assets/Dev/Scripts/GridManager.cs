using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Dev.Scripts
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] public List<GridData> gridList;
        [SerializeField] public List<GemType> gemTypeList;
        [SerializeField] public GameObject tilePrefab;
        [SerializeField] public float tileSize;
        
        private static GridManager _instance;
        public static GridManager Instance => _instance;
        
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            
            GenerateGrid();
        }

        private void GenerateGrid()
        {
            GemPool.Instance.Initialize(gemTypeList,GetTotalGemCount());

            foreach (var gridData in gridList)
            {
                int rowCount = gridData.rowCount;
                int columnCount = gridData.columnCount;

                for (int row = 0; row < rowCount; row++)
                {
                    for (int column = 0; column < columnCount; column++)
                    {
                        Vector3 spawnPosition = new Vector3(column * tileSize, 1f, row * tileSize) + gridData.position;
                        
                        var tile = (GameObject)Instantiate(tilePrefab, transform);
                        tile.transform.position = spawnPosition;
                    }
                }
            }
        }
        
        private int GetTotalGemCount()
        {
            int totalGemCount = 0;

            foreach (var gridData in gridList)
            {
                int rowCount = gridData.rowCount;
                int columnCount = gridData.columnCount;
                int gemCountPerGrid = rowCount * columnCount;

                totalGemCount += gemCountPerGrid;
            }

            return totalGemCount;
        }
        
    }



    [CustomEditor(typeof(GridManager))]
    public class GridManagerEditor : Editor
    {
        private SerializedProperty _gridListProp;
        private SerializedProperty _tilePrefabProp;
        private SerializedProperty _tileSizeProp;
        private SerializedProperty _gemTypeList;

        private int _newRowCount = 4;
        private int _newColumnCount = 4;
        private Vector3 _newGridPosition = Vector3.zero;

        private void OnEnable()
        {
            _gridListProp = serializedObject.FindProperty("gridList");
            _tilePrefabProp = serializedObject.FindProperty("tilePrefab");
            _tileSizeProp = serializedObject.FindProperty("tileSize");
            _gemTypeList = serializedObject.FindProperty("gemTypeList");
        }
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            EditorGUILayout.PropertyField(_tilePrefabProp);
            EditorGUILayout.PropertyField(_tileSizeProp);
            EditorGUILayout.PropertyField(_gridListProp, true);
            EditorGUILayout.PropertyField(_gemTypeList);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Add Grid", EditorStyles.boldLabel);
            _newRowCount = EditorGUILayout.IntField("Row Count", _newRowCount);
            _newColumnCount = EditorGUILayout.IntField("Column Count", _newColumnCount);
            _newGridPosition = EditorGUILayout.Vector3Field("Grid Position", _newGridPosition);
            
            if (GUILayout.Button("Add Grid"))
            {
                AddGrid();
            }

            EditorGUILayout.Space();

            if (GUILayout.Button("Generate Grids"))
            {
                GenerateGrids();
            }

            if (GUILayout.Button("Reset Grids"))
            {
                ResetGrids();
            }

            serializedObject.ApplyModifiedProperties();
        }
        
        private void OnSceneGUI()
        {
            GridManager gridManager = (GridManager)target;

            foreach (var gridData in gridManager.gridList)
            {
                Handles.color = Color.blue;
                Vector3 gridPosition = gridData.position + new Vector3(gridData.columnCount * gridManager.tileSize * 0.5f - gridManager.tileSize * 0.5f,
                    1f,
                    gridData.rowCount * gridManager.tileSize * 0.5f - gridManager.tileSize * 0.5f);
                Handles.DrawWireCube(gridPosition, new Vector3(gridData.columnCount * gridManager.tileSize,
                    1f,
                    gridData.rowCount * gridManager.tileSize));

                Handles.color = new Color(0f, 0.5f, 1f, 0.3f); 

                Vector3 bottomLeft = new Vector3(gridPosition.x - gridData.columnCount * gridManager.tileSize * 0.5f,0 ,gridPosition.z - gridData.rowCount * gridManager.tileSize * 0.5f);
                Vector3 bottomRight = new Vector3(gridPosition.x + gridData.columnCount * gridManager.tileSize * 0.5f,0, gridPosition.z - gridData.rowCount * gridManager.tileSize * 0.5f);
                Vector3 topRight = new Vector3(gridPosition.x + gridData.columnCount * gridManager.tileSize * 0.5f,0, gridPosition.z + gridData.rowCount * gridManager.tileSize * 0.5f);
                Vector3 topLeft = new Vector3(gridPosition.x - gridData.columnCount * gridManager.tileSize * 0.5f, 0,gridPosition.z + gridData.rowCount * gridManager.tileSize * 0.5f);

                Handles.DrawSolidRectangleWithOutline(new Vector3[] { bottomLeft, bottomRight, topRight, topLeft },
                    new Color(0f, 0.5f, 1f, 0.3f), Color.clear);






            }
        }

        private void AddGrid()
        {
            GridManager gridManager = (GridManager)target;

            GridData newGrid = new GridData(_newRowCount, _newColumnCount, _newGridPosition);
            gridManager.gridList.Add(newGrid);
        }

        private void GenerateGrids()
        {
            GridManager gridManager = (GridManager)target;

            foreach (var gridData in gridManager.gridList)
            {
                int rowCount = gridData.rowCount;
                int columnCount = gridData.columnCount;

                for (int row = 0; row < rowCount; row++)
                {
                    for (int column = 0; column < columnCount; column++)
                    {
                        Vector3 spawnPosition = new Vector3(column * gridManager.tileSize, 1f, row * gridManager.tileSize) + gridData.position;
                        
                        var tile = (GameObject)PrefabUtility.InstantiatePrefab(gridManager.tilePrefab,
                            gridManager.transform);
                        tile.transform.position = spawnPosition;

                        GemType randomGemType = gridManager.gemTypeList[Random.Range(0, gridManager.gemTypeList.Count)];
                        var gemInstance = Instantiate(randomGemType.model, tile.transform);
                        gemInstance.transform.localPosition = Vector3.zero;
                        tile.name = randomGemType.gemName;
                    }
                }
            }
        }

        private void ResetGrids()
        {
            GridManager gridManager = (GridManager)target;

            if (gridManager.transform.childCount < 1)
                return;

            while (gridManager.transform.childCount > 0)
            {
                var child = gridManager.transform.GetChild(0);
                DestroyImmediate(child.gameObject);
            }
        }
    }
}