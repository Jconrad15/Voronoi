using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voronoi
{
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

        public Cell SeedCell { get; protected set; }

        public bool IsSeedCell { get; protected set; }

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

        public void SetAsSeedCell()
        {
            if (IsSeedCell)
            {
                Debug.LogWarning("Changing seed cell when cell is a seedCell itself.");
                return;
            }
            IsSeedCell = true;
            SeedCell = this;

            // Select the color for this seed
            CurrentColor = Utility.RandomColor(1);
            Renderer cube_r = cube.GetComponent<Renderer>();
            cube_r.material.color = CurrentColor;

            //cube_r.material.SetFloat("_Glossiness", 1f);
            cube_r.material.SetFloat("_Metallic", 0.5f);

            // Change height for the seed cells
            Vector3 changedHeight = CellPosition;
            changedHeight.y = Random.value * CellMetrics.heightScale;
            CellPosition = changedHeight;
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
        }

    }
}