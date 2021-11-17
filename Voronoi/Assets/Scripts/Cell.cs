using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voronoi
{
    public class Cell : MonoBehaviour
    {

        public Vector3 position;
        public int index;

        private GameObject cube;

        public void InitializeCell(int x, int z, int index, Material cellMaterial)
        {
            position = new Vector3(x, 0, z);
            this.index = index;

            cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = position;
            cube.transform.SetParent(this.transform);

            Renderer cube_r = cube.GetComponent<Renderer>();
            cube_r.material = cellMaterial;
        }

        public void SetColor(Color newColor)
        {
            cube.GetComponent<Renderer>().material.color = newColor;
        }

        public Color GetColor()
        {
            return cube.GetComponent<Renderer>().material.color;
        }


    }
}