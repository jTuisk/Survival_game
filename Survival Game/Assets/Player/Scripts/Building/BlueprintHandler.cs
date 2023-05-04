using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Player.Controller;
using Game.UI;

namespace Game.Player.Building
{
    public class BlueprintHandler : MonoBehaviour
    {
        [SerializeField] bool objectIsPlaced = false;

        [SerializeField] GameObject toObject;
        //Required materials to build..

        private Material defaultBlueprintMaterial;
        [SerializeField] Material errorBlueprintMaterial;
        [SerializeField] List<GameObject> triggeredObjects;
        [SerializeField] Vector3 OffSetValue;

        private Transform player;


        // Start is called before the first frame update
        void Awake()
        {
            defaultBlueprintMaterial = gameObject.GetComponent<Renderer>().material;
            triggeredObjects = new List<GameObject>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!objectIsPlaced)
            {
                if(transform.parent.tag.Equals("Player"))
                {
                    if(player == null)
                    {
                        player = transform.parent;
                    }
                    //Calculate it so that it never goes under the surface.
                    transform.position = player.position + (player.transform.forward * PlayerSettings.Instance.distanceFromPlayer) + Camera.main.transform.forward + OffSetValue;

                    if (PlayerController.Instance.InputManager.Q)
                    {
                        RotateObject(1f);
                    }
                    else if (PlayerController.Instance.InputManager.E)
                    {
                        RotateObject(-1f);
                    }

                    if (PlayerController.Instance.InputManager.Esc)
                    {
                        Cancel();
                    }

                    if (PlayerController.Instance.InputManager.AttackIsPressed)
                    {
                        PlaceObject();
                    }
                }
            }
        }

        public void PlaceRequiredItems()
        {
            if (true) //check if player has any required item.
            {

                if (true) // check if building part got all required items.
                {
                    Debug.Log("Replace blueprint with real object!");
                    GameObject go = Instantiate(toObject, transform.parent);
                    go.transform.position = transform.position;
                    go.transform.rotation = transform.rotation;
                    go.transform.localScale = transform.localScale;
                    
                    Destroy(gameObject);
                }
            }
        }

        public void Cancel()
        {
            UIManager.Instance.ChangeUI(0);
            Destroy(gameObject);
        }

        public void PlaceObject()
        {
            Debug.Log("Place object!");
            if (!objectIsPlaced)
            {
                objectIsPlaced = true;
                gameObject.transform.parent = PlayerSettings.Instance.placeBuildingObjectTo;
                UIManager.Instance.ChangeUI(0);
            }
        }

        private void RotateObject(float dir)
        {
            transform.Rotate(transform.rotation.x * dir * PlayerSettings.Instance.rotationSpeed * Time.deltaTime, transform.rotation.y, transform.rotation.z, Space.Self);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!triggeredObjects.Contains(other.gameObject))
            {
                triggeredObjects.Add(other.gameObject);
                //Check if snap object is building part if so, snap to it..
            }

            gameObject.GetComponent<Renderer>().material = triggeredObjects.Count == 0 || objectIsPlaced ? defaultBlueprintMaterial : errorBlueprintMaterial;
        }


        private void OnTriggerExit(Collider other)
        {
            triggeredObjects.Remove(other.gameObject);

            gameObject.GetComponent<Renderer>().material = triggeredObjects.Count == 0 || objectIsPlaced ? defaultBlueprintMaterial : errorBlueprintMaterial;
        }
    }

}

