using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voronoi
{

    public enum DevelopmentType { urban, suburban, rural };
    public enum Direction { N, E, S, W };

    public class Cell : MonoBehaviour
    {
        private Vector3 cellPosition;
        public Vector3 CellPosition
        {
            get
            {
                return cellPosition;
            }
            set
            {
                cellPosition = value;
                cube.transform.position = cellPosition;
            }
        }
        public int index;

        public Color CurrentColor { get; protected set; }

        private GameObject cube;
        private GameObject model;

        private bool isRoad;
        public bool IsRoad
        {
            get
            {
                return isRoad;
            }
        }

        // N, E, S, W
        private bool[] roadDirections = new bool[4];

        public void AddRoad(Direction direction)
        {
            isRoad = true;

            roadDirections[(int)direction] = true;
        }

        public void AddRoad(Direction direction1, Direction direction2)
        {
            isRoad = true;

            roadDirections[(int)direction1] = true;
            roadDirections[(int)direction2] = true;
        }

        public int RoadDirectionCount()
        {
            int count = 0;
            for (int i = 0; i < roadDirections.Length; i++)
            {
                if (roadDirections[i] == true)
                {
                    count += 1;
                }
            }
            return count;
        }

        public Cell SeedCell { get; protected set; }

        public bool IsSeedCell { get; protected set; }

        public DevelopmentType DevType { get; set; }

        public void InitializeCell(int x, int z, int index, Material cellMaterial)
        {
            cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

            CellPosition = new Vector3(x, 0, z);
            this.index = index;

            cube.transform.SetParent(this.transform);

            Renderer cube_r = cube.GetComponent<Renderer>();
            cube_r.material = cellMaterial;
            cube_r.material.color = CellMetrics.defaultColor;
            CurrentColor = CellMetrics.defaultColor;

            // Set initial road value to false
            isRoad = false;
        }

        public void SetColor(Color newColor)
        {
            CurrentColor = newColor;
            cube.GetComponent<Renderer>().material.color = CurrentColor;
        }

        public Color GetColor()
        {
            return CurrentColor;
        }

        public void SetAsSeedCell(Color setColor)
        {
            if (IsSeedCell)
            {
                Debug.LogWarning("Changing seed cell when cell is a seedCell itself.");
                return;
            }
            IsSeedCell = true;
            SeedCell = this;

            // Select the color for this seed
            CurrentColor = setColor;
            Renderer cube_r = cube.GetComponent<Renderer>();
            cube_r.material.color = CurrentColor;

            //cube_r.material.SetFloat("_Glossiness", 1f);
            cube_r.material.SetFloat("_Metallic", 0.5f);

            // Change height for the seed cells
/*            Vector3 changedHeight = CellPosition;
            changedHeight.y = Random.value * CellMetrics.heightScale;
            CellPosition = changedHeight;*/
        }

        public void UpdateSeedCell(Cell seedCell)
        {
            if (IsSeedCell) 
            { 
                Debug.LogWarning("Changing seed cell when cell is a seedCell itself.");
                return;
            }

            SeedCell = seedCell;
            // Change height to match the seed cell
            Vector3 changedHeight = CellPosition;
            changedHeight.y = seedCell.CellPosition.y;
            CellPosition = changedHeight;

            // Change development type to match the seed cell
            DevType = seedCell.DevType;
        }

        public void PlaceModel(GameObject modelPrefab)
        {
            model = Instantiate(modelPrefab);
            model.transform.SetParent(this.transform);

            Vector3 modelPosition = cellPosition;
            modelPosition.y += cube.transform.localScale.y / 2f;
            model.transform.position = modelPosition;


        }



    }
}