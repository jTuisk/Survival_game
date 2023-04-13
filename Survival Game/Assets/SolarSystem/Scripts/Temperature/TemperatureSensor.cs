using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.SolarSystem.Temperature
{
    public class TemperatureSensor : MonoBehaviour
    {
        [SerializeField] protected bool shootRays = true;
        [SerializeField] TemperatureSensor[] sunSensors;
        [SerializeField] TemperatureSensor[] sensors;

        [SerializeField] SensorTemperatureData temperatureData;

        [SerializeField] bool shootRaysInInterval = true;
        [SerializeField] float intervalTime = 10f;
        [SerializeField, ReadOnly] float nextInterval = 0f;

        [SerializeField] bool averageOutSensors = true;
        [SerializeField, Range(0f, 2f)] float averageOutMultiplier = 0.01f;
        [SerializeField] int numberOfNearestSensors = 3;

        [SerializeField] TemperatureSensor[] nearestSensors;

        [SerializeField] bool applySeasonModifier = true;

        public float GetCurrentTemperature()
        {
            return temperatureData.currentTemperature;
        }

        private void Awake()
        {
            if (shootRays)
            {
                sunSensors = FindObjectsOfType<SunTemperatureSensor>();
                sensors = transform.parent.transform.GetComponentsInChildren<TemperatureSensor>();
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            temperatureData.currentTemperature = temperatureData.startTemperature;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (!shootRays)
                return;

            if (shootRaysInInterval)
            {
                if(nextInterval > 0f)
                {
                    nextInterval -= Time.deltaTime;
                }
                else
                {
                    CastRaysToSun();
                }
            }
            else
            {
                CastRaysToSun();
            }
            UpdateNearestSensors(numberOfNearestSensors);
        }

        public void UpdateSensorList()
        {
            sensors = transform.parent.transform.GetComponentsInChildren<TemperatureSensor>();
        }

        private void CastRaysToSun()
        {
            nextInterval = intervalTime;


            if (sunSensors.Length > 0)
            {
                float[] tempChange = new float[sunSensors.Length];
                float finalTempChange = 0f;
                //foreach(TemperatureSensor sensor in sunSensors)
                for (int i = 0; i < sunSensors.Length; i++)
                {
                    RaycastHit hit;
                    Vector3 directionToSensor = (sunSensors[i].transform.position - transform.position).normalized;

                    if (Physics.Raycast(transform.position, directionToSensor, out hit, Mathf.Infinity))
                    {
                        if(hit.collider.name == "Sun")
                        {
                            Debug.DrawRay(transform.position, directionToSensor * hit.distance, Color.green);
                            //IncreaseTemperature();
                            tempChange[i] = sunSensors[i].temperatureData.currentTemperature * sunSensors[i].temperatureData.temperatureStrength;
                        }
                        else
                        {
                            Debug.DrawRay(transform.position, directionToSensor * 10000f, Color.red);
                            //ReducingTemperature();
                            tempChange[i] = Universe.VoidTemperature * Universe.VoidTemperatureStrength;
                        }
                    }
                    else
                    {
                        Debug.DrawRay(transform.position, directionToSensor * 10000f, Color.red);
                        //ReducingTemperature();
                        tempChange[i] = Universe.VoidTemperature * Universe.VoidTemperatureStrength;
                    }
                }
                for(int i = 0; i < tempChange.Length; i++)
                {
                    finalTempChange += tempChange[i];
                }
                finalTempChange /= tempChange.Length; // Add season/month modifier!!
                //Debug.Log($"final sun temp change: {finalTempChange}");
                ChangeTemp(finalTempChange);
                AverageOutSensorsTemperature();
            }
        }

        private void ChangeTemp(float amo)
        {
            if (amo < 0)
            {
                ReducingTemperature(amo);
            }
            else
            {
                IncreaseTemperature(amo);
            }
        }

        private void IncreaseTemperature(float amo = 1)
        {
            temperatureData.currentTemperature = Mathf.Min(temperatureData.currentTemperature + amo, temperatureData.maxTempature);
        }

        private void ReducingTemperature(float amo = 1)
        {
            temperatureData.currentTemperature = Mathf.Max(temperatureData.currentTemperature - amo, temperatureData.minTemperature);
        }

        private void UpdateNearestSensors(int n)
        {
            nearestSensors = new TemperatureSensor[n];

            for (int i = 0; i < nearestSensors.Length; i++)
            {
                foreach (TemperatureSensor sensor in sensors)
                {
                    bool skipSensor = false;

                    for (int j = 0; j < nearestSensors.Length; j++)
                    {
                        if (nearestSensors[j] == sensor)
                        {
                            skipSensor = true;
                        }
                    }

                    if (sensor == this || skipSensor)
                        continue;

                    if (nearestSensors[i] == null)
                    {
                        nearestSensors[i] = sensor;
                        continue;
                    }

                    float distanceToCurrentSensor = Vector3.Distance(transform.position, nearestSensors[i].transform.position);
                    float distanceToSensor = Vector3.Distance(transform.position, sensor.transform.position);

                    if (distanceToCurrentSensor > distanceToSensor)
                    {
                        nearestSensors[i] = sensor;
                    }
                }
            }
        }

        private void AverageOutSensorsTemperature()
        {
            if (averageOutSensors)
            {
                float tempChange = 0f;
                
                for(int i = 0; i < nearestSensors.Length; i++)
                {
                    if (nearestSensors[i] == null)
                        continue;

                    float tempDifference = (temperatureData.currentTemperature * temperatureData.temperatureStrength) - (nearestSensors[i].temperatureData.currentTemperature * nearestSensors[i].temperatureData.temperatureStrength);
                    tempChange += tempDifference * averageOutMultiplier;
                }
                ChangeTemp(tempChange);
            }
        }
    }
}
