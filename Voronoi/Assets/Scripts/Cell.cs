using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voronoi
{

    public enum DevelopmentType { urban, suburban, rural };

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
            model.transform.position = modelPosition;
            modelPosition.y += cube.transform.localScale.y / 2f;


        }



    }
}