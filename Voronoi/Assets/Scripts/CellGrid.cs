using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voronoi
{
    public class CellGrid : MonoBehaviour
    {
        [SerializeField]
        private Material cellMaterial;

        public int xSize = 10;
        public int zSize = 10;
        public int cellCount;

        private Cell[] cells;

        private float[] steps;

        // Start is called before the first frame update
        void OnEnable()
        {
            CreateGrid();
            JumpFlood();
        }

        private void CreateGrid()
        {
            cellCount = xSize * zSize;
            cells = new Cell[cellCount];

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
            cell.InitializeCell(x, z, index, cellMaterial);
            return cell;
        }

        private void JumpFlood()
        {
            steps = new float[4]
            {
                cellCount/2f,
                cellCount/4f,
                cellCount/8f,
                1
            };

            Vector2[] neighborRef = new Vector2[8]
            {
                new Vector2(-1, -1),
                new Vector2(-1, 0),
                new Vector2(-1, 1),

                new Vector2(0, -1),
                //new Vector2(0, 0),
                new Vector2(0, 1),

                new Vector2(1, -1),
                new Vector2(1, 0),
                new Vector2(1, 1)
            };

            // Iterate over each step size
            for (int k = 0; k < steps.Length; k++)
            {
                // For all x and z coordinates
                for (int x = 0; x < xSize; x++)
                {
                    for (int z = 0; z < zSize; z++)
                    {
                        // For neighboring cells based on step size
                        foreach (Vector2 offset in neighborRef)
                        {
                            Cell neighborq = GetCell(
                                x + ((int)offset.x * k),
                                z + ((int)offset.y * k));

                            // Compare neighbor color to cell color
                            // recolor based on distance to seeds

                        }

                    }
                }
            }

        }

        private Cell GetCell(int x, int z)
        {
            int i = (x * xSize) + z;

            return cells[i];
        }


    }
}