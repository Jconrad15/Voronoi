using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voronoi
{
    public static class Utility
    {

        public static Color RandomColor(float alpha)
        {
            Mathf.Clamp01(alpha);

            float r = Random.value;
            float g = Random.value;
            float b = Random.value;

            Color newColor = new Color(r, g, b, alpha);

            return newColor;
        }


    }
}