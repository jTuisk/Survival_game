using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Player.Items;
using Game.Player.Inventory;
using Game.Player.Controller;
using Game.UI.Building;

namespace Game.Player.Building
{
    public class ConstructionPartBlueprint : MonoBehaviour
    {
        public enum ConstructionPartType {Wall, Ceiling, Foundation, Other}

        [SerializeField, ReadOnly] bool blueprintIsPlaced = false;

        [SerializeField] ConstructionPartType type;
        public ConstructionPartType Type => type;

        [Header("Blueprint of ")]
        [SerializeField] GameObject finishedPart;
        [SerializeField] List<ConstructionRequiredItem> requiredItems;

        [Header("Snapping")]
        [SerializeField] Transform snapPointsParent;
        GameObject[] snapPoints;
        Transform lastSnapPointHit;
        bool falseSnapPoint;

        [Header("Position/Rotation/Raycast")]
        [SerializeField, ReadOnly] Vector3 rayHitPosition;
        [SerializeField, ReadOnly] Quaternion rayHitRotation;
        [SerializeField, ReadOnly] Vector3 rotationAmount;
        [SerializeField] float rotationSpeed = 50f;
        [SerializeField] LayerMask layerMasks;
        const int defaultLayer = 9;
        const int ignoreRayCastLayer = 2;

        [Header("Materials")]
        [SerializeField] Renderer goRenderer;
        [SerializeField] Material falseBlueprintMaterial;
        Material defaultBlueprintMaterial;

        [Header("UI")]
        [SerializeField] Transform uiParent;
        [SerializeField] GameObject uiSlotPrefab;

        [Header("Others")]
        [SerializeField, ReadOnly] List<GameObject> triggeredObjects;
        [SerializeField, ReadOnly] List<GameObject> triggeredSnapObjects;

        float actionTimer = 0f;
        const float actionInterval = 0.5f;

        void Start()
        {
            defaultBlueprintMaterial = gameObject.GetComponent<Renderer>().material;
            triggeredObjects = new List<GameObject>();
            InitializeSnapPoints();
            SetLayer(gameObject, ignoreRayCastLayer);
        }

        void Update()
        {
            if (!blueprintIsPlaced && GameManager.Instance.gameStatus != GameManager.GameStatus.Ingame_placing_blueprints)
                Destroy(gameObject);

            if (actionTimer > 0)
            {
                actionTimer -= Time.deltaTime;
            }

            HandlePlayerInput();
            InitializeRequirmentUI();
        }

        void FixedUpdate()
        {
            if (blueprintIsPlaced || GameManager.Instance.gameStatus != GameManager.GameStatus.Ingame_placing_blueprints)
                return;

            ShootRaycast();
            MoveBlueprint();
        }

        void SetLayer(GameObject go, int layer)
        {
            go.layer = layer;
            
            foreach(Transform child in go.transform)
            {
                child.gameObject.layer = layer;

                Transform grandchild;
                if(child.TryGetComponent<Transform>(out grandchild))
                {
                    SetLayer(child.gameObject, layer);
                }
            }
        }

        public void PlaceRequiredItems()
        {
            foreach(var reqItem in requiredItems)
            {
                InventorySystem.Instance.RemoveItems(reqItem.item, ref reqItem.quantity);
            }

            requiredItems.RemoveAll(reqItem => reqItem.quantity <= 0);

            InitializeRequirmentUI();

            if (requiredItems.Count == 0)
            {
                GameObject go = Instantiate(finishedPart, transform.parent);
                go.transform.position = transform.position;
                go.transform.rotation = transform.rotation;
                go.transform.localScale = transform.localScale;

                Destroy(gameObject);
            }
        }

        void HandlePlayerInput()
        {
            if (blueprintIsPlaced || GameManager.Instance.gameStatus != GameManager.GameStatus.Ingame_placing_blueprints)
                return;

            if (PlayerController.Instance.InputManager.E)
            {
                RotateBlueprint(1);
            }

            else if (PlayerController.Instance.InputManager.R)
            {
                RotateBlueprint(-1);
            }

            if (PlayerController.Instance.InputManager.Esc)
            {
                CancelBuilding();
            }

            if (PlayerController.Instance.InputManager.AttackIsPressed)
            {
                Placeblueprint();
            }
        }

        void RotateBlueprint(int dir)
        {
            rotationAmount += Vector3.up * dir * rotationSpeed * Time.deltaTime;
        }

        void InitializeRequirmentUI()
        {
            uiParent.gameObject.SetActive(blueprintIsPlaced && requiredItems.Count != 0);

            if (!blueprintIsPlaced)
                return;

            if (uiParent != null)
            {
                foreach(Transform t in uiParent.transform)
                {
                    Destroy(t.gameObject);
                }
                if(uiSlotPrefab  != null)
                {
                    foreach (var reqItem in requiredItems)
                    {
                        GameObject slot = Instantiate(uiSlotPrefab, uiParent);
                        slot.GetComponent<MissingItemsUIHandler>().UpdateData(reqItem.item.itemData.icon, reqItem.quantity);
                    }
                }
            }
        }

        public void Placeblueprint()
        {
            actionTimer = actionInterval;
            if (CanPlace())
            {
                blueprintIsPlaced = true;
                SetLayer(gameObject, defaultLayer);
                gameObject.transform.parent = PlayerSettings.Instance.placeBuildingObjectTo;
                GameManager.Instance.gameStatus = GameManager.GameStatus.Ingame;
            }
        }

        public void CancelBuilding()
        {
            GameManager.Instance.gameStatus = GameManager.GameStatus.Ingame;
            Destroy(gameObject);
        }

        protected virtual void ShootRaycast()
        {
            if (GameManager.Instance.gameStatus != GameManager.GameStatus.Ingame_placing_blueprints)
                return;

            Transform camera = Camera.main.transform;

            RaycastHit hit;

            Debug.DrawRay(camera.position, camera.forward * PlayerSettings.Instance.maxDistanceFromPlayer, Color.red);

            if (Physics.Raycast(camera.position, camera.forward, out hit, PlayerSettings.Instance.maxDistanceFromPlayer, layerMasks))
            {
                if(hit.transform.tag == "SnapPoint")
                {
                    Debug.DrawLine(camera.position, hit.point, Color.cyan);

                    if (lastSnapPointHit != null && lastSnapPointHit == hit.transform)
                        return;

                    falseSnapPoint = false;

                    Transform hitTransform = hit.transform;


                    ConstructionPartBlueprint hitBlueprint = hitTransform.GetComponentInParent<ConstructionPartBlueprint>();
                    Vector3 centerPoint = GetSnapPointsCenterPoint();
                    Vector3 hitCenterPoint = hitBlueprint.GetSnapPointsCenterPoint();


                    lastSnapPointHit = hitTransform;
                    rayHitPosition = hitTransform.position;
                    rayHitRotation = hitTransform.parent.rotation;

                    switch (type)
                    {
                        case ConstructionPartType.Wall:
                            if (hitBlueprint.Type == ConstructionPartType.Wall)
                            {
                                if (hitTransform.name == "Left" || hitTransform.name == "Right")
                                {
                                    rayHitPosition += GetDirection(hitCenterPoint, hitTransform.position) * Vector3.Distance(hitCenterPoint, hitTransform.position); //Move object next to snapped object.
                                    rayHitPosition += GetDirection(centerPoint, transform.position) * hitTransform.localPosition.y / 2; //Set height same as snapped object.
                                }
                                else if (hitTransform.name == "Bottom")
                                {
                                    falseSnapPoint = true;
                                }
                            }
                            if(hitBlueprint.Type == ConstructionPartType.Foundation)
                            {
                                rayHitRotation = Quaternion.LookRotation((hitTransform.position - hitCenterPoint), Vector3.up);
                                rotationAmount = Vector3.zero;
                            }
                            break;

                        case ConstructionPartType.Foundation:
                            if(hitBlueprint.Type == ConstructionPartType.Foundation)
                            {
                                rayHitPosition += GetDirection(hitCenterPoint, hitTransform.position) * Vector3.Distance(hitCenterPoint, hitTransform.position);
                            }
                            else if(hitBlueprint.Type == ConstructionPartType.Wall)
                            {
                                if (hitTransform.name == "Bottom")
                                {
                                    rayHitPosition += GetDirection(snapPoints[0].transform.position, centerPoint) * Vector3.Distance(snapPoints[0].transform.position, centerPoint);
                                    rotationAmount = Vector3.zero;
                                }
                                else
                                {
                                    falseSnapPoint = true;
                                }
                            }
                            break;

                        case ConstructionPartType.Ceiling:
                            if (hitBlueprint.Type == ConstructionPartType.Ceiling)
                            {
                                rayHitPosition += GetDirection(hitCenterPoint, hitTransform.position) * Vector3.Distance(hitCenterPoint, hitTransform.position); 
                            }
                            else if (hitBlueprint.Type == ConstructionPartType.Wall)
                            {
                                if (hitTransform.name == "Top") // || hitTransform.name == "Left" || hitTransform.name == "Rigt"
                                {
                                    rayHitPosition += GetDirection(snapPoints[0].transform.position, centerPoint) * Vector3.Distance(snapPoints[0].transform.position, centerPoint);
                                    rotationAmount = Vector3.zero;
                                }
                                else
                                {
                                    falseSnapPoint = true;
                                }
                            }
                            break;

                        default:
                            Debug.Log("Construction part type snap point settings is not set!");
                            break;
                    }
                }
                else
                {
                    Debug.DrawLine(camera.position, hit.point, Color.green);
                    lastSnapPointHit = null;
                    rayHitPosition = hit.point;
                }
            }
            else
            {
                lastSnapPointHit = null;
                rayHitPosition = camera.position + camera.forward * PlayerSettings.Instance.maxDistanceFromPlayer;
            }
        }

        Vector3 GetFurthestSnapPointFrom(Vector3 fromPos)
        {
            Vector3 furthest = Vector3.zero;
            float furthestDistance = float.MaxValue;

            foreach(GameObject go in snapPoints)
            {
                float distance = Vector3.Distance(fromPos, go.transform.position);
                if(distance < furthestDistance)
                {
                    furthestDistance = distance;
                    furthest = go.transform.position;
                }
            }
            return furthest;
        }

        public Vector3 GetSnapPointsCenterPoint()
        {
            Vector3 centerPoint = Vector3.zero;

            foreach (GameObject point in snapPoints)
                centerPoint += point.transform.position;

            centerPoint /= snapPoints.Length;

            return centerPoint;
        }

        Vector3 GetDirection(Vector3 from, Vector3 to)
        {
            return (to - from).normalized;
        }

        void CleanLists()
        {
            triggeredObjects.RemoveAll(x => x == null);
            triggeredSnapObjects.RemoveAll(x => x == null);
        }

        void MoveBlueprint()
        {
            if (blueprintIsPlaced)
                return;

            Quaternion finalRotation = Quaternion.Euler(rotationAmount);

            if(lastSnapPointHit != null && !falseSnapPoint)
            {
                finalRotation *= rayHitRotation;
            }

            transform.rotation = finalRotation;

            transform.position = rayHitPosition;
        }

        void InitializeSnapPoints()
        {
            if (snapPointsParent == null)
                return;

            snapPoints = new GameObject[snapPointsParent.childCount];

            for(int i = 0; i < snapPoints.Length; i++)
            {
                snapPoints[i] = snapPointsParent.GetChild(i).gameObject;
            }
        }

        bool CanPlace()
        {
            CleanLists();

            if (blueprintIsPlaced)
                return false;

            if (triggeredObjects.Count == 0)
                return true;

            if (falseSnapPoint && lastSnapPointHit != null)
                return false;

            if (triggeredObjects.Count == 1 && triggeredObjects[0].layer == 9 && lastSnapPointHit != null)
                return true;

            if (triggeredObjects.Count == 2 && triggeredObjects[0].layer == 9 && triggeredObjects[1].layer == 9 && lastSnapPointHit != null)
                return true;

            return false;
        }

        void UpdateMaterial()
        {
            goRenderer.material = CanPlace() || blueprintIsPlaced ? defaultBlueprintMaterial : falseBlueprintMaterial;
        }


        void OnTriggerEnter(Collider other)
        {
            if (!triggeredObjects.Contains(other.gameObject))
            {
                if(other.tag != "SnapPoint")
                {
                    triggeredObjects.Add(other.gameObject);
                }
                else
                {
                    triggeredSnapObjects.Add(other.gameObject);
                }
            }

            UpdateMaterial();
        }

        void OnTriggerExit(Collider other)
        {
            triggeredObjects.Remove(other.gameObject);
            triggeredSnapObjects.Remove(other.gameObject);
            UpdateMaterial();
        }


        [System.Serializable]
        public class ConstructionRequiredItem
        {
            public ItemScriptableObject item;
            public int quantity;
        }
    }
}
