using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Player.Controller;
using Game.SolarSystem.Temperature;


namespace Game.Player
{
    public class Temperature : MonoBehaviour
    {

        [SerializeField, ReadOnly] float currentTemperature = 0f;
        [SerializeField, ReadOnly] TemperatureSensor[] nearestSensors;
        [SerializeField] int nearestSensorsAmount = 3;
        [SerializeField] float maxDistance = 5f;

        [SerializeField] bool useIntervals = true;
        [SerializeField] float t = 0f;
        [SerializeField] float intervalT = 2f;

        [SerializeField, ReadOnly] PlayerController playerController;
        [SerializeField, ReadOnly] TemperatureSensorHandler planetSensorHandler;

        public float CurrentTemperature
        {
            get { return currentTemperature; }
            private set { currentTemperature = value; }
        }

        private void LateUpdate()
        {
            if (useIntervals)
            {
                t -= Time.deltaTime;

                if (t <= 0)
                {
                    UpdateTemperature();
                }
            }
            else
            {
                UpdateTemperature();
            }

        }

        private void UpdateControllers()
        {
            playerController = transform.parent.GetComponentInChildren<PlayerController>();
            planetSensorHandler = playerController.celestialBody.GetComponentInChildren<TemperatureSensorHandler>();
        }

        private void UpdateTemperature()
        {
            if (playerController != null && planetSensorHandler != null)
            {
                nearestSensors = GetNearestSensors();
                CurrentTemperature = CalculateTemperature();
                t = intervalT;
            }
            else
            {
                UpdateControllers();
            }
        }

        private TemperatureSensor[] GetNearestSensors()
        {
            TemperatureSensor[] sensors = planetSensorHandler.transform.GetComponentsInChildren<TemperatureSensor>();
            TemperatureSensor[] nearest = new TemperatureSensor[nearestSensorsAmount];

            for (int i = 0; i < nearest.Length; i++)
            {
                foreach (TemperatureSensor sensor in sensors)
                {
                    bool skipSensor = false;

                    for (int j = 0; j < nearest.Length; j++)
                    {
                        if (nearest[j] == sensor)
                        {
                            skipSensor = true;
                        }
                    }

                    if (skipSensor)
                        continue;

                    if (nearest[i] == null)
                    {
                        nearest[i] = sensor;
                        continue;
                    }

                    float distanceToCurrentSensor = Vector3.Distance(transform.position, nearest[i].transform.position);
                    float distanceToSensor = Vector3.Distance(transform.position, sensor.transform.position);

                    if (distanceToCurrentSensor > distanceToSensor)
                    {
                        nearest[i] = sensor;
                    }
                }
            }
            return nearest;
        }

        private float CalculateTemperature()
        {
            float finalTemperature = 0f;

            float[] strength = new float[nearestSensors.Length];
            float strengthMultiplier = 1f; // we use this value to make sure sum of strength is always equal or close to 1.

            float sensorStrengthSum = 0;
            for(int i = 0; i < strength.Length; i++)
            {
                float distance = Vector3.Distance(transform.position, nearestSensors[i].transform.position);
                strength[i] = distance / maxDistance;
                strength[i] = Mathf.Clamp01(strength[i]);
                strength[i] = 1f - strength[i];
                sensorStrengthSum += strength[i];
            }
            strengthMultiplier = 1 / sensorStrengthSum;

            for (int i = 0; i < nearestSensors.Length; i++)
            {
                finalTemperature += nearestSensors[i].GetCurrentTemperature() * strength[i] * strengthMultiplier;
            }

            return finalTemperature;
        }

    }
}


