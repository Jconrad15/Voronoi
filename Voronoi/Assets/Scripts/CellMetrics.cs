using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voronoi
{
    public static class CellMetrics
    {
        public static Color defaultColor = Color.white;
        public static float heightScale = 1.5f;

        /// <summary>
        /// Returns the opposite direction.
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static Direction OppositeDirection(Direction direction)
        {
            if (direction == Direction.N)
            {
                return Direction.S;
            }
            else if (direction == Direction.S)
            {
                return Direction.N;
            }
            else if (direction == Direction.E)
            {
                return Direction.W;
            }
            else //if (direction == Direction.W)
            {
                return Direction.E;
            }
        }

        public static Direction RandomTurnDirection(Direction direction)
        {
            if (Random.value > 0.5)
            {
                return LeftDirection(direction);
            }
            return RightDirection(direction);
        }

        public static Direction LeftDirection(Direction direction)
        {
            int value = (int)direction - 1;

            if (value == -1) { return Direction.W; }

            return (Direction)value;
        }

        public static Direction RightDirection(Direction direction)
        {
            int value = (int)direction + 1;

            if (value == 4) { return Direction.N; }

            return (Direction)value;
        }
    }
}