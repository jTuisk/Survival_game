using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Misc
{
    public class FollowPlanetsCamera : MonoBehaviour
    {
        public bool Enabled = true;
        bool wasFollowing = true;

        public GameObject planet;
        public Vector3 distance;
        public Quaternion rotation;


        Vector3 defaultPosition;
        Quaternion defaultRotation;

        private void OnValidate()
        {
            defaultPosition = transform.position;
            defaultRotation = transform.rotation;
        }


        // Update is called once per frame
        void Update()
        {
            if(wasFollowing && !enabled)
            {
                transform.DetachChildren();
                transform.position = defaultPosition;
                wasFollowing = false;
            }

            if (enabled && !wasFollowing)
            {
                transform.parent = planet.transform;
                wasFollowing = true;
            }

            if(enabled && wasFollowing)
            {
                transform.position = planet.transform.position + distance;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 360f);
            }
        }


    }
}