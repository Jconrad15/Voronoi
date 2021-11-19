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

        public static Color ColorShift(Color color, float hAmount, float sAmount, float vAmount)
        {
            Color.RGBToHSV(color, out float h, out float s, out float v);

            h = Mathf.Clamp01(h + hAmount);
            s = Mathf.Clamp01(s + sAmount);
            v = Mathf.Clamp01(v + vAmount);

            color = Color.HSVToRGB(h, s, v);

            return color;
        }


    }
}