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
        [SerializeField, ReadOnly] bool blueprintIsPlaced = false;
        
        [Header("Blueprint of ")]
        [SerializeField] GameObject finishedPart;
        [SerializeField] List<ConstructionRequiredItem> requiredItems;

        [Header("Snapping")]
        [SerializeField] Transform snapPointsParent;
        GameObject[] snapPoints;

        [Header("Position/Rotation/Raycast")]
        [SerializeField, ReadOnly] Vector3 rayHitPosition;
        [SerializeField] LayerMask layerMasks;
        const int defaultLayer = 9;
        const int ignoreRayCastLayer = 2;
        [SerializeField] float rotationSpeed = 50f;

        [Header("Materials")]
        [SerializeField] Renderer goRenderer;
        [SerializeField] Material falseBlueprintMaterial;
        private Material defaultBlueprintMaterial;

        [Header("UI")]
        [SerializeField] Transform uiParent;
        [SerializeField] GameObject uiSlotPrefab;

        [Header("Others")]
        [SerializeField, ReadOnly] List<GameObject> triggeredObjects;
        [SerializeField, ReadOnly] List<GameObject> triggeredSnapObjects;
        float actionTimer = 0f;
        float actionInterval = 0.5f;


        protected virtual void Start()
        {
            defaultBlueprintMaterial = gameObject.GetComponent<Renderer>().material;
            triggeredObjects = new List<GameObject>();
            InitializeSnapPoints();
            SetLayer(gameObject, true, ignoreRayCastLayer);
        }

        protected virtual void Update()
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

        protected virtual void FixedUpdate()
        {
            if (blueprintIsPlaced || GameManager.Instance.gameStatus != GameManager.GameStatus.Ingame_placing_blueprints)
                return;

            ShootRaycast();
            MoveBlueprint();
        }

        private void SetLayer(GameObject go, bool childs, int layer)
        {
            go.layer = layer;
            if (childs)
            {
                foreach(var collider in go.GetComponentsInChildren<Collider>())
                {
                    collider.gameObject.layer = layer;
                }
            }
        }

        public void PlaceRequiredItems()
        {
            foreach(var reqItem in requiredItems)
            {
                Debug.Log($"reqItem: {reqItem.item.name}, quantity: {reqItem.quantity}, player has: {InventorySystem.Instance.ItemQuantity(reqItem.item)} ");

                InventorySystem.Instance.RemoveItems(reqItem.item, ref reqItem.quantity);
            }

            requiredItems.RemoveAll(reqItem => reqItem.quantity <= 0);

            Debug.Log($"reqCount: {requiredItems.Count}");
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

        protected virtual void HandlePlayerInput()
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

        protected virtual void RotateBlueprint(int dir)
        {
            transform.Rotate(transform.rotation * transform.up * rotationSpeed * dir * Time.deltaTime);
        }

        private void InitializeRequirmentUI()
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
            Debug.Log("Place blueprint! "+ CanPlace());
            actionTimer = actionInterval;
            if (CanPlace())
            {
                blueprintIsPlaced = true;
                SetLayer(gameObject, true, defaultLayer);
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
                    Vector3 vector3 = hit.point.normalized;

                    rayHitPosition = hit.point; //calculate new position value that does denays overlapping.
                    transform.rotation = hit.transform.rotation; //hit.transform.parent.rotation;
                }
                else
                {
                    Debug.DrawLine(camera.position, hit.point, Color.green);
                    rayHitPosition = hit.point;
                }
            }
            else
            {
                rayHitPosition = camera.position + camera.forward * PlayerSettings.Instance.maxDistanceFromPlayer;
            }
        }

        protected virtual void CleanLists()
        {
            triggeredObjects.RemoveAll(x => x == null);
            triggeredSnapObjects.RemoveAll(x => x == null);
        }

        protected virtual void MoveBlueprint()
        {
            if (blueprintIsPlaced)
                return;

            Vector3 finalPosition = rayHitPosition;
            //Debug.Log($"snapObjects: {triggeredSnapObjects.Count}");

            CleanLists();
            if (triggeredSnapObjects.Count == 1)
            {
                finalPosition.y = triggeredSnapObjects[0].transform.position.y;
            }

            transform.position = finalPosition;
        }

        protected virtual void InitializeSnapPoints()
        {
            if (snapPointsParent == null)
                return;

            snapPoints = new GameObject[snapPointsParent.childCount];

            for(int i = 0; i < snapPoints.Length; i++)
            {
                snapPoints[i] = snapPointsParent.GetChild(i).gameObject;
            }
        }

        protected virtual bool CanPlace()
        {
            if (blueprintIsPlaced)
                return false;

            if (triggeredObjects.Count == 0)
                return true;

            if (triggeredObjects.Count == 1 && triggeredObjects[0].layer == 9) // figure better way to achive this..
                return true;

            if (triggeredObjects.Count == 2 && triggeredObjects[0].layer == 9 && triggeredObjects[1].layer == 9) // figure better way to achive this..
                return true;

            return false;
        }

        private void UpdateMaterial()
        {
            goRenderer.material = CanPlace() || blueprintIsPlaced ? defaultBlueprintMaterial : falseBlueprintMaterial;
        }


        protected virtual void OnTriggerEnter(Collider other)
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

        protected virtual void OnTriggerExit(Collider other)
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
