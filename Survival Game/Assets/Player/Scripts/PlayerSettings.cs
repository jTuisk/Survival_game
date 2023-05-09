using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game.Player
{
    public class PlayerSettings : MonoBehaviour
    {
        public static PlayerSettings Instance { get; private set; }

        public bool canMove = true;
        public bool canRotateCamera = true;

        [Header("Building")]
        public bool buildingPartSnapping = true;
        public float distanceFromPlayer = 1.5f;
        public float maxDistanceFromPlayer = 3f;
        public float rotationSpeed = 1f;
        public Transform placeBuildingObjectTo;
        public bool canBuild = true;
        public bool canFinishBlueprints = true;



        


        private void Awake()
        {
            // If there is an instance, and it's not me, delete myself.

            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }
    }

}
