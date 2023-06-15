using System;
using UnityEngine;

namespace Dev.Scripts
{
    [Serializable]
    public class GridData
    {
        public int rowCount;
        public int columnCount;
        public Vector3 position;

        public GridData(int rowCount, int columnCount, Vector3 position)
        {
            this.rowCount = rowCount;
            this.columnCount = columnCount;
            this.position = position;
        }
    }

}