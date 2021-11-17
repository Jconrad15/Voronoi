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

        public void InitializeCell(int x, int z, int index)
        {
            position = new Vector3(x, 0, z);
            this.index = index;

            cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = position;
            cube.transform.SetParent(this.transform);

        }


    }
}