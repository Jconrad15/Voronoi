using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Voronoi
{
    public class CellGrid : MonoBehaviour
    {
        [SerializeField]
        private ModelHandler modelHandler;

        [SerializeField]
        private Material cellMaterial;

        private int xSize = 100;
        private int zSize = 100;
        private int cellCount;

        private int seedCount = 50;

        private Cell[] cells;

        private float[] steps;

        private Color[] colors;

        private int[] seedIndices;

        // Start is called before the first frame update
        private void OnEnable()
        {
            GenerateWorld();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Clear();
                GenerateWorld();
            }
        }

        private void GenerateWorld()
        {
            //System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            //stopwatch.Start();

            GenerateColors();
            CreateGrid();
            JumpFlood();

            CreateRoads();
            PlaceCellModels();


            /*
            stopwatch.Stop();
            System.TimeSpan ts = stopwatch.Elapsed;
            string elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Debug.Log("RunTime " + elapsedTime);
            */
        }

        /// <summary>
        /// Destory all previous gameobjects.
        /// </summary>
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
            seedIndices = SelectSeedCells();
            int counter = 0;
            foreach (int seedIndex in seedIndices)
            {
                cells[seedIndex].SetAsSeedCell(colors[counter]);
                counter += 1;
            }

            // Determine seed cell development type
            for (int i = 0; i < seedIndices.Length; i++)
            {
                // Get seed cell
                Cell seedCell = cells[seedIndices[i]];
                // Set Development
                seedCell.DevType = Utility.GetRandomEnum<DevelopmentType>();
            }

        }

        /// <summary>
        /// Returns an integer array for selected cell indicies.
        /// </summary>
        /// <returns></returns>
        private int[] SelectSeedCells()
        {
            // limit seeds by number of cells
            if (seedCount > cellCount)
            {
                Debug.LogError("seedCount is set to greater than the cellCount");
                return null;
            }

            int[] indices = new int[seedCount];
            for (int i = 0; i < seedCount; i++)
            {
                int randomValue = Random.Range(0, cellCount);
                while (indices.Contains(randomValue))
                {
                    randomValue = Random.Range(0, cellCount);
                }

                indices[i] = randomValue;
            }

            return indices;
        }

        /// <summary>
        /// Creates and returns a cell.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="z"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private Cell CreateCell(int x, int z, int index)
        {
            GameObject cell_go = new GameObject("Cell " + x + ", " + z + ", i=" + index);
            cell_go.transform.SetParent(this.transform);

            Cell cell = cell_go.AddComponent<Cell>();
            cell.InitializeCell(x, z, index, cellMaterial, this);
            return cell;
        }

        private void JumpFlood()
        {
            steps = new float[7]
            {
                cellCount,
                cellCount/2f,
                cellCount/4f,
                cellCount/8f,
                1,
                2,
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
                                        Vector3.Distance(cell.CellPosition, cell.SeedCell.CellPosition);
                                    float distNeighborSeed =
                                        Vector3.Distance(cell.CellPosition, neighborCell.SeedCell.CellPosition);

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

        public Cell GetCell(int x, int z)
        {
            if (z > zSize || z < 0 ||
                x > xSize || x < 0) 
            { 
                return null; 
            }

            int i = (x * xSize) + z;

            return cells[i];
        }

        public Cell GetCell(Cell cell, Direction direction)
        {
            int xStart = (int)cell.CellPosition.x;
            int zStart = (int)cell.CellPosition.z;

            return direction switch
            {
                Direction.N => GetCell(xStart + 1, zStart),
                Direction.E => GetCell(xStart, zStart + 1),
                Direction.S => GetCell(xStart - 1, zStart),
                Direction.W => GetCell(xStart, zStart - 1),
                _ => null,
            };
        }

        private void GenerateColors()
        {
            colors = new Color[seedCount];

            for (int i = 0; i < seedCount; i++)
            {
                Color newColor;
                if (i == 0)
                {
                    // Random color for the first index
                    newColor = Color.HSVToRGB(
                        Random.value,
                        0.7f,
                        0.6f);
                }
                else
                {
                    // New color is based on previous color
                    newColor = Utility.ColorShift(colors[i - 1],
                        1f / seedCount, 0, 0);//Random.Range(-.02f, .02f), Random.Range(-.02f, .02f));
                }

                colors[i] = newColor;
            }
        }

        private void CreateRoads()
        {
            // Determine number of roads
            int roadCount = 30;
            for (int i = 0; i < roadCount; i++)
            {
                // Select starting road cell
                Cell startCell = cells[Random.Range(0, cells.Length)];

                int roadLength = Random.Range(10, 20);
                Cell currentCell = startCell;
                Direction direction = Utility.GetRandomEnum<Direction>();
                for (int j = 0; j < roadLength; j++)
                {
                    Cell nextCell = null;

                    while (nextCell == null)
                    {
                        currentCell.AddRoad(direction);

                        // Get the next cell
                        nextCell = GetCell(currentCell, direction);
                    }

                    // Set the next cell to current cell
                    currentCell = nextCell;
                }
            }

        }

        private void PlaceCellModels()
        {
            for (int i = 0; i < cells.Length; i++)
            {
                Cell cell = cells[i];

                // Place a model from modelHandler at every cell
                cell.PlaceModel(
                    modelHandler.GetModel(
                        cell.DevType, cell.IsSeedCell, cell.IsRoad));
            }
        }

    }
}