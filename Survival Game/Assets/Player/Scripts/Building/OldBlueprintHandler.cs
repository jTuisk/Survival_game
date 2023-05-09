using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Player.Controller;
using Game.Player.Items;

namespace Game.Player.Building
{
    public class OldBlueprintHandler : MonoBehaviour
    {
        private Transform player;
        [SerializeField] bool objectIsPlaced = false;

        [SerializeField] GameObject toObject;
        //Required materials to build..

        private Material defaultBlueprintMaterial;
        [SerializeField] Material errorBlueprintMaterial;
        [SerializeField] List<GameObject> triggeredObjects;
        [SerializeField] float offSetDistance;

        [SerializeField] float groundCheckDistanceOffSet = 0.01f;
        [SerializeField] Transform raycastPointsParent;
        Transform[] rayCastPoints;

        float upOffSetValue = 0f;

        [SerializeField] float rotation;
        [SerializeField] float rotationSpeed = 3f;

        [SerializeField] BlueprintSettings blueprintSettings;
        public BlueprintSettings BlueprintSettings => blueprintSettings;

        OldBlueprintHandler snappedTo;

        // Start is called before the first frame update
        void Awake()
        {
            defaultBlueprintMaterial = gameObject.GetComponent<Renderer>().material;
            triggeredObjects = new List<GameObject>();
        }

        private void Start()
        {
            InitRaycastPoints();
        }

        // Update is called once per frame
        void Update()
        {
            if (!objectIsPlaced && GameManager.Instance.gameStatus != GameManager.GameStatus.Ingame_placing_blueprints)
                Destroy(gameObject);

            if (!objectIsPlaced)
            {
                if (transform.parent.tag.Equals("Player"))
                {
                    if (player == null)
                    {
                        player = transform.parent;
                    }


                    RaycastHit[] hits = new RaycastHit[rayCastPoints.Length];
                    float[] distance = new float[hits.Length];

                    for (int i = 0; i < rayCastPoints.Length; i++)
                    {
                        if (Physics.Raycast(rayCastPoints[i].position + transform.up, -transform.up, out hits[i]))
                        {
                            //Debug.DrawRay(rayCastPoints[i].position + transform.up, -transform.up, Color.red);
                            distance[i] = 1f - hits[i].distance;
                            //Debug.Log($"i:{i} -> {distance[i]}");
                        }
                    }


                    if (triggeredObjects.Count == 0) // Raycast.distances results are negative numbers.
                    {
                        float newOffsetValue = float.MinValue;

                        foreach (float d in distance)
                        {
                            newOffsetValue = d > newOffsetValue ? d : newOffsetValue;
                        }
                        newOffsetValue += groundCheckDistanceOffSet;

                        upOffSetValue = newOffsetValue < upOffSetValue ? newOffsetValue : upOffSetValue;
                    }
                    else
                    {
                        float newOffsetValue = float.MaxValue;
                        foreach (float d in distance)
                        {
                            newOffsetValue = d < newOffsetValue ? d : newOffsetValue;
                        }
                        newOffsetValue -= groundCheckDistanceOffSet; 

                        upOffSetValue = newOffsetValue > upOffSetValue ? newOffsetValue : upOffSetValue;
                    }

                    //Debug.Log($"final offset value: {upOffSetValue}-> {transform.up * upOffSetValue}/{transform.up}");

                    transform.position = GetBlueprintPosition(player, upOffSetValue);
                    //transform.position = (transform.up * upOffSetValue) + (player.position + player.transform.forward * PlayerSettings.Instance.distanceFromPlayer) + (Camera.main.transform.forward * offSetDistance* PlayerSettings.Instance.distanceFromPlayer);

                   /*if (PlayerController.Instance.InputManager.Q)
                    {
                        RotateObject(1f);
                    }

                    else */if (PlayerController.Instance.InputManager.E)
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

        private Vector3 GetBlueprintPosition(Transform from, float groundOffSet)
        {
            Vector3 camera = (Camera.main.transform.forward * offSetDistance * PlayerSettings.Instance.distanceFromPlayer);
            Vector3 player = (from.position + from.forward * PlayerSettings.Instance.distanceFromPlayer);
            Vector3 position = (transform.up * upOffSetValue);

            RaycastHit hit;

            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit))
            {
                Debug.DrawLine(Camera.main.transform.position, hit.transform.position);
                OldBlueprintHandler hitBpHandler = hit.transform.gameObject.GetComponent<OldBlueprintHandler>();
                if (hitBpHandler != null) // 
                {
                    snappedTo = hitBpHandler;
                    Vector3 finalPosition;

                    if (blueprintSettings.SnapToPosition(snappedTo.BlueprintSettings, out finalPosition))
                    {
                        transform.rotation = Quaternion.AngleAxis(rotation, Vector3.up) * snappedTo.transform.rotation;
                        return finalPosition;
                    }
                }
                else
                {
                    snappedTo = null;
                }
            }
            return camera + player + position;
        }

        private bool CanPlace()
        {
            if (objectIsPlaced)
                return false;

            if(triggeredObjects.Count == 0)
                return true;

            Debug.Log($"triggerCount: {triggeredObjects.Count}");

            Vector3 temp;
            if (triggeredObjects.Count == 1 && snappedTo != null && blueprintSettings.SnapToPosition(snappedTo.BlueprintSettings, out temp))
            {
                return true;
            }

            if(triggeredObjects.Count == 2 && snappedTo != null && blueprintSettings.SnapToPosition(snappedTo.BlueprintSettings, out temp))
            {
                if ((triggeredObjects[0].layer == LayerMask.NameToLayer("CelestialBody") && triggeredObjects[1].layer == LayerMask.NameToLayer("BuildingBlueprint")) ||
                    (triggeredObjects[0].layer == LayerMask.NameToLayer("BuildingBlueprint") && triggeredObjects[1].layer == LayerMask.NameToLayer("CelestialBody")))
                    return true;
            }


            return false;
        }

        private void InitRaycastPoints()
        {
            rayCastPoints = new Transform[raycastPointsParent.childCount];

            for(int i = 0; i < rayCastPoints.Length; i++)
            {
                rayCastPoints[i] = raycastPointsParent.GetChild(i).transform;
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
            GameManager.Instance.gameStatus = GameManager.GameStatus.Ingame;
            Destroy(gameObject);
        }

        public void PlaceObject()
        {
            if (CanPlace())
            {
                objectIsPlaced = true;
                gameObject.transform.parent = PlayerSettings.Instance.placeBuildingObjectTo;
                GameManager.Instance.gameStatus = GameManager.GameStatus.Ingame;
            }
        }

        private void RotateObject(float dir)
        {
            rotation += rotationSpeed * Time.deltaTime;
        }

        public void ClearHitList()
        {
            triggeredObjects.Clear();
            gameObject.GetComponent<Renderer>().material = CanPlace() || objectIsPlaced ? defaultBlueprintMaterial : errorBlueprintMaterial;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!triggeredObjects.Contains(other.gameObject))
            {
                triggeredObjects.Add(other.gameObject);
            }

            gameObject.GetComponent<Renderer>().material = CanPlace() || objectIsPlaced ? defaultBlueprintMaterial : errorBlueprintMaterial;
        }


        private void OnTriggerExit(Collider other)
        {
            triggeredObjects.Remove(other.gameObject);
            gameObject.GetComponent<Renderer>().material = CanPlace() || objectIsPlaced ? defaultBlueprintMaterial : errorBlueprintMaterial;
        }
    }

    [System.Serializable]
    public class BlueprintSettings
    {
        public bool canSnapTo = true;
        public float maxSnapDistance = 0.1f;
        public Transform[] snapPoints;

        public bool SnapToPosition(BlueprintSettings other, out Vector3 toPos)
        {
            Vector3 closestFrom = Vector3.zero;
            Vector3 closestTo = Vector3.zero;
            float closestDistance = float.MaxValue;

            if(snapPoints != null && other.snapPoints != null)
            {
                for(int i = 0; i < snapPoints.Length; i++)
                {
                    for(int j = 0; j < other.snapPoints.Length; j++)
                    {
                        float distance = Vector3.Distance(snapPoints[i].position, other.snapPoints[j].position);

                        Debug.DrawLine(snapPoints[i].position, other.snapPoints[j].position, Color.red);
                        if (distance < closestDistance)
                        {
                            closestFrom = snapPoints[i].position;
                            closestTo = other.snapPoints[j].position + other.snapPoints[j].localPosition/2.05f;
                            //closestTo = snapPoints[j].position + snapPoints[j].localPosition/2.1f;
                            closestDistance = distance;
                        }
                    }
                }
                Debug.DrawLine(closestFrom, closestTo, Color.green);
                toPos = closestTo;
                return true;
            }
            toPos = closestTo;
            return false;
        }
    }
}

