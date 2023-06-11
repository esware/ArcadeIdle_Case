using System;
using Dev.Scripts;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridManager))]
public class GridManagerEditor : Editor
{
    private SerializedProperty _rowCountProp;
    private SerializedProperty _columnCountProp;
    private SerializedProperty _tilePrefabProp;
    private SerializedProperty _tileSizeProp;

    private void OnEnable()
    {
        _rowCountProp = serializedObject.FindProperty("rowCount");
        _columnCountProp = serializedObject.FindProperty("columnCount");
        _tilePrefabProp = serializedObject.FindProperty("tilePrefab");
        _tileSizeProp = serializedObject.FindProperty("tileSize");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(_rowCountProp);
        EditorGUILayout.PropertyField(_columnCountProp);
        EditorGUILayout.PropertyField(_tilePrefabProp);
        EditorGUILayout.PropertyField(_tileSizeProp);

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
                Vector3 spawnPosition = new Vector3(column * gridManager.tileSize, 0f, row * gridManager.tileSize);
                GameObject tile = (GameObject)PrefabUtility.InstantiatePrefab(gridManager.tilePrefab, gridManager.transform);
                tile.transform.position = spawnPosition;
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
