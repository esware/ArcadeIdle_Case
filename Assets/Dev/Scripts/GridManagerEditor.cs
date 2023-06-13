using System;
using Dev.Scripts;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

[CustomEditor(typeof(GridManager))]
public class GridManagerEditor : Editor
{
    private SerializedProperty _rowCountProp;
    private SerializedProperty _columnCountProp;
    private SerializedProperty _tilePrefabProp;
    private SerializedProperty _tileSizeProp;
    private SerializedProperty _gemTypeList;

    private void OnEnable()
    {
        _rowCountProp = serializedObject.FindProperty("rowCount");
        _columnCountProp = serializedObject.FindProperty("columnCount");
        _tilePrefabProp = serializedObject.FindProperty("tilePrefab");
        _tileSizeProp = serializedObject.FindProperty("tileSize");
        _gemTypeList = serializedObject.FindProperty("gemTypeList");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(_rowCountProp);
        EditorGUILayout.PropertyField(_columnCountProp);
        EditorGUILayout.PropertyField(_tilePrefabProp);
        EditorGUILayout.PropertyField(_tileSizeProp);
        EditorGUILayout.PropertyField(_gemTypeList);

        if (GUILayout.Button("Generate Grid"))
        {
            GenerateGrid();
        }
        
        if (GUILayout.Button("Reset Grid"))
        {
            Reset();
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void GenerateGrid()
    {
        GridManager gridManager = (GridManager)target;

        for (int row = 0; row < gridManager.rowCount; row++)
        {
            for (int column = 0; column < gridManager.columnCount; column++)
            {
                Vector3 spawnPosition = new Vector3(column * gridManager.tileSize, 1f, row * gridManager.tileSize);
                var grid = (GameObject)PrefabUtility.InstantiatePrefab(gridManager.tilePrefab, gridManager.transform);
                grid.transform.position = spawnPosition;


                GemType randomGemType = gridManager.gemTypeList[Random.Range(0, gridManager.gemTypeList.Count)];
                var gemInstance = Instantiate(randomGemType.model, grid.transform);
                gemInstance.transform.localScale = Vector3.one;
                grid.name = randomGemType.gemName;
            }
        }
    }
    

    private void Reset()
    {
        GridManager gridManager = (GridManager) target;

        while (gridManager.transform.childCount > 0)
        {
            Transform child = gridManager.transform.GetChild(0);
            DestroyImmediate(child.gameObject);
        }
    }
    
}
