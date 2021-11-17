using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Voronoi
{
    public class CellGrid : MonoBehaviour
    {
        [SerializeField]
        private Material cellMaterial;

        private int xSize = 50;
        private int zSize = 50;
        private int cellCount;

        private int seedCount = 30;

        private Cell[] cells;

        private float[] steps;

        // Start is called before the first frame update
        private void OnEnable()
        {
            CreateGrid();
            JumpFlood();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Clear();

                CreateGrid();
                JumpFlood();
            }
        }

        private void Clear()
        {
            foreach (Cell cell in cells)
            {
                Destroy(cell.gameObject);
            }
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

            // Determine seed cells
            int[] seedIndices = SelectSeedCells();
            foreach (int seedIndex in seedIndices)
            {
                cells[seedIndex].SetAsSeedCell();
            }
        }

        private int[] SelectSeedCells()
        {
            // limit seeds by number of cells
            if (seedCount > cellCount)
            {
                Debug.LogWarning("seedCount is set to greater than the cellCount");
                seedCount = xSize / 2;
            }

            int[] seedIndices = new int[seedCount];
            for (int i = 0; i < seedCount; i++)
            {
                int randomValue = Random.Range(0, cellCount);
                while (seedIndices.Contains(randomValue))
                {
                    randomValue = Random.Range(0, cellCount);
                }

                seedIndices[i] = randomValue;
            }

            return seedIndices;
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

            Color defaultColor = CellMetrics.defaultColor;

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
                            // Neighbor positions
                            int neighborX = x + ((int)offset.x * k);
                            int neighborZ = z + ((int)offset.y * k);

                            // Only get in bounds neighbors
                            if (neighborX < 0 || neighborX > xSize - 1) { continue; }
                            if (neighborZ < 0 || neighborZ > zSize - 1) { continue; }

                            // Get cells
                            // Skip if this is a seed cell
                            Cell cell = GetCell(x, z);
                            if (cell.IsSeedCell) { continue; }
                            Cell neighborCell = GetCell(neighborX, neighborZ);

                            // Compare neighbor color to cell color
                            if (cell.CurrentColor == defaultColor)
                            {
                                if (neighborCell.CurrentColor != defaultColor)
                                {
                                    // Change cell's color to neighbor's color
                                    cell.SetColor(neighborCell.GetColor());
                                    cell.UpdateSeedCell(neighborCell.SeedCell);
                                }
                                // Otherwise do nothing if both are default color
                            }
                            else
                            {
                                // The cell does not have a default color
                                if (neighborCell.CurrentColor != defaultColor)
                                {
                                    if (cell.SeedCell == null) 
                                    {
                                        Debug.LogError("SeedCell is null");
                                        return; 
                                    }
                                    // Both cells are colored
                                    float distCurrentSeed =
                                        Vector3.Distance(cell.position, cell.SeedCell.position);
                                    float distNeighborSeed =
                                        Vector3.Distance(cell.position, neighborCell.SeedCell.position);

                                    // If the neighbor's seed is closer than the current seed
                                    if (distCurrentSeed > distNeighborSeed)
                                    {
                                        // Change the seed cell and color
                                        cell.SetColor(neighborCell.CurrentColor);
                                        cell.UpdateSeedCell(neighborCell.SeedCell);
                                    }

                                }
                                // Do nothing if neighbor is default color
                            }
                        } // End neighbors
                    } // End z
                } // End x
            } // End step iteration
        }

        private Cell GetCell(int x, int z)
        {
            int i = (x * xSize) + z;

            return cells[i];
        }


    }
}