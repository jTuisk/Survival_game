using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Player.Items;
using Game.Player.Controller;

namespace Game.Player.Building
{
    public class ContstructionBlueprint : MonoBehaviour
    {
        [SerializeField, ReadOnly] bool blueprintIsPlaced = false;
        
        [Header("Blueprint of ")]
        [SerializeField] GameObject finishedPart;
        [SerializeField] ConstructionRequiredItem[] requiredItems;

        [Header("Snapping")]
        [SerializeField] Transform snapPointsParent;
        GameObject[] snapPoints;

        [Header("Position/Rotation/Raycast")]
        [SerializeField] bool canPlaceToSnapPoint = true;
        [SerializeField, ReadOnly] Vector3 rayHitPosition;
        [SerializeField] LayerMask layerMasks;
        const int defaultLayer = 9;
        const int ignoreRayCastLayer = 2;

        [Header("Materials")]
        [SerializeField] Renderer renderer;
        [SerializeField] Material falseBlueprintMaterial;
        private Material defaultBlueprintMaterial;

        //[Header("UI")]

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
        }

        protected virtual void FixedUpdate()
        {
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

        protected virtual void HandlePlayerInput()
        {
            if (GameManager.Instance.gameStatus != GameManager.GameStatus.Ingame_placing_blueprints)
                return;
            /*if (PlayerController.Instance.InputManager.Q)
                    {
                        RotateObject(1f);
                    }

                    else */
            if (PlayerController.Instance.InputManager.E)
            {
                RotateBlueprint();
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

        protected virtual void RotateBlueprint()
        {
            Debug.LogError("Rotation!");
        }

        public void Placeblueprint()
        {
            if (GameManager.Instance.gameStatus != GameManager.GameStatus.Ingame_placing_blueprints)
                return;

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

            Debug.DrawRay(camera.position, camera.forward * 10f, Color.red);

            if (Physics.Raycast(camera.position, camera.forward, out hit, PlayerSettings.Instance.maxDistanceFromPlayer, layerMasks))
            {
                Debug.DrawLine(camera.position, hit.point, Color.green);
                rayHitPosition = hit.point;
            }
            else
            {
                rayHitPosition = camera.position + camera.forward * PlayerSettings.Instance.maxDistanceFromPlayer;
            }
        }

        protected virtual void MoveBlueprint()
        {
            if (blueprintIsPlaced)
                return;

            Vector3 finalPosition = rayHitPosition;
            Debug.Log($"snapObjects: {triggeredSnapObjects.Count}");

            if(triggeredSnapObjects.Count == 1)
            {
                rayHitPosition.y = triggeredSnapObjects[0].transform.position.y;
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


            return false;
        }

        private void UpdateMaterial()
        {
            renderer.material = CanPlace() || blueprintIsPlaced ? defaultBlueprintMaterial : falseBlueprintMaterial;
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
