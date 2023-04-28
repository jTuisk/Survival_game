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
