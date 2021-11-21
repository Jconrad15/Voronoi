using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voronoi
{
    public class ModelHandler : MonoBehaviour
    {
        [SerializeField]
        private GameObject urbanPrefab;

        [SerializeField]
        private GameObject suburbanPrefab;

        [SerializeField]
        private GameObject ruralPrefab;

        [SerializeField]
        private GameObject urbanCenterPrefab;

        [SerializeField]
        private GameObject suburbanCenterPrefab;

        [SerializeField]
        private GameObject ruralCenterPrefab;

        [SerializeField]
        private GameObject roadStraightPrefab;

        /// <summary>
        /// Returns a model gameobject for a cell.
        /// </summary>
        /// <param name="devType"></param>
        /// <param name="isSeedCell"></param>
        /// <returns></returns>
        public GameObject GetModel(DevelopmentType devType, bool isSeedCell, bool isRoad)
        {
            if (isRoad)
            {
                return roadStraightPrefab;
            }
            else
            {
                // Is not a road
                // Seed cells get center prefabs
                if (isSeedCell == true)
                {
                    switch (devType)
                    {
                        case DevelopmentType.urban:
                            return urbanCenterPrefab;

                        case DevelopmentType.suburban:
                            return suburbanCenterPrefab;

                        case DevelopmentType.rural:
                            return ruralCenterPrefab;

                        default:
                            Debug.LogError("No prefab??");
                            return null;
                    }
                }

                // Otherwise cell is not a seedCell
                switch (devType)
                {
                    case DevelopmentType.urban:
                        return urbanPrefab;

                    case DevelopmentType.suburban:
                        return suburbanPrefab;

                    case DevelopmentType.rural:
                        return ruralPrefab;

                    default:
                        Debug.LogError("No prefab??");
                        return null;
                }
            }
        }


    }
}