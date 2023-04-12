using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.SolarSystem.Planet;

namespace Game.SolarSystem.Temperature
{
    public class TemperatureSensorHandler : MonoBehaviour
    {
        [SerializeField] bool generateSensors;
        [SerializeField] GameObject sensorPrefab;
        [SerializeField] IcosahedronPlanet planet;
        [SerializeField, Range(0.5f, 1.5f)] float offsetMultiplier = 1f;

        private void Start()
        {
            GenerateSensors();
        }

        private void GenerateSensors()
        {
            if (generateSensors && sensorPrefab != null && planet != null)
            {
                for (int i = 0; i < planet.GetDefaultVertices().Count; i++)
                {
                    Instantiate(sensorPrefab, transform);
                }
                UpdateSensorList();
                UpdateLocations();
            }
        }

        public void UpdateSensorList()
        {
            foreach (Transform t in transform)
            {
                t.GetComponent<TemperatureSensor>().UpdateSensorList();
            }
        }

        public void UpdateLocations()
        {
            List<Vector3> defaultPositions = planet.GetDefaultVertices();

            if (planet != null)
            {
                if (defaultPositions.Count == transform.childCount)
                {
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        transform.GetChild(i).transform.localPosition = planet.GetSurfacePoint(defaultPositions[i]) * offsetMultiplier;
                    }
                }
            }
        }
    }
}
