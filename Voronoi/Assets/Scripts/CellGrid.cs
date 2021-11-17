using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voronoi
{
    public class CellGrid : MonoBehaviour
    {

        public int xSize = 10;
        public int zSize = 10;

        private Cell[] cells;

        // Start is called before the first frame update
        void OnEnable()
        {
            cells = new Cell[xSize * zSize];

            int index = 0;

            for (int x = 0; x < xSize; x++)
            {
                for (int z = 0; z < zSize; z++)
                {
                    cells[index] = CreateCell(x, z, index);
                    index += 1;
                }
            }



        }

        private Cell CreateCell(int x, int z, int index)
        {
            GameObject cell_go = new GameObject("Cell " + x + ", " + z + ", i=" + index);
            cell_go.transform.SetParent(this.transform);

            Cell cell = cell_go.AddComponent<Cell>();
            cell.InitializeCell(x, z, index);
            return cell;
        }

    }
}