using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Dev.Scripts
{
    public class GridManager : MonoBehaviour
    {
        public GameObject tilePrefab; 
        public int rowCount; 
        public int columnCount;
        public float tileSize; 

        [SerializeField]
        public List<GemType> gemTypeList;
        
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
            for (int row = 0; row < rowCount; row++)
            {
                for (int column = 0; column < columnCount; column++)
                {
                    Vector3 spawnPosition = new Vector3(column * tileSize, 1f, row * tileSize);
                    GameObject tile = (GameObject)Instantiate(tilePrefab, transform);
                    tile.transform.position = spawnPosition;
                    
                }
            }
        }
    }
}