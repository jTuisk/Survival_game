using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using Game.Player;
using Game.Player.Input;
using Game.Player.Items;
using Game.Player.Inventory;
using Game.Player.Building;
using Game.UI;

namespace Game.Player.Controller
{
    [System.Serializable]
    public class InteractionHandler
    {
        private InputManager inputManager;
        private InteractionSettingsScriptableObject settingsData;

        private float defaultInteractionT = 0f;
        private float defaultIntervalTime = 0.5f;

        public InteractionHandler(InputManager inputManager, InteractionSettingsScriptableObject settings)
        {
            this.inputManager = inputManager;
            this.settingsData = settings;
        }

        public void Handle()
        {
            if(defaultInteractionT > 0f)
            {
                defaultInteractionT -= Time.deltaTime;
            }

            //Better way would be to make function that handles every raycast action, so we don't need to shoot multiple rays...
            HandleItemPickup();
            HandleBuildingBlueprint();
            HandleTabButton();
        }


        private void HandleBuildingBlueprint()
        {
            var settings = settingsData.building;
            if (settings.timer > 0f)
            {
                settings.timer -= Time.deltaTime;
                Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * settings.distance, Color.blue);
            }
            else
            {
                if (inputManager.Interact && PlayerSettings.Instance.canFinishBlueprints)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, settings.distance, settings.itemLayer.value))
                    {
                        
                        ConstructionPartBlueprint buildingObject = hit.transform.GetComponent<ConstructionPartBlueprint>();
                        settings.timer = settings.interval;
                        buildingObject?.PlaceRequiredItems();
                    }
                }
                if (inputManager.BlockIsPressed && GameManager.Instance.gameStatus == GameManager.GameStatus.Ingame_placing_blueprints)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, settings.distance, settings.itemLayer.value))
                    {

                        ConstructionPartBlueprint buildingObject = hit.transform.GetComponent<ConstructionPartBlueprint> ();
                        settings.timer = settings.interval;
                        if (buildingObject != null)
                            GameObject.Destroy(buildingObject.gameObject);
                    }
                }
            }
        }

        private void HandleItemPickup()
        {
            var settings = settingsData.itemPickup;

            if (settings.timer > 0f)
            {
                settings.timer -= Time.deltaTime;
                Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * settings.distance, Color.blue);
            }
            else
            {
                if (inputManager.Interact && settings.canPickup)
                {
                    RaycastHit hit;

                    if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, settings.distance, settings.itemLayer.value))
                    {
                        InteractiveItem iItem = hit.transform.GetComponent<InteractiveItem>();
                        ChestHandler ch = hit.transform.GetComponent<ChestHandler>();

                        if(iItem != null)
                        {
                            Item item = iItem.itemData.itemData;
                            Debug.Log($"id: {item.id}, name:{item.name}");
                            if (iItem.Pickup())
                            {
                                settings.timer = settings.cooldown;
                                Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * settings.distance, Color.green);
                            }
                            else
                            {
                                settings.timer = settings.failCooldown;
                                Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * settings.distance, Color.cyan);
                            }
                        }

                        if(ch != null)
                        {
                            ch.OpenChest();
                        }

                    }
                    else
                    {
                        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * settings.distance, Color.red);
                    }
                }
            }
        }

        private void HandleDisplayUIs()
        {

        }

        private void HandleTabButton()
        {
            var settings = settingsData.interfaceses;
            if (settings.timer > 0)
            {
                settings.timer -= Time.deltaTime;
            }
            else
            {
                if (settings.canOpen)
                {
                    switch (GameManager.Instance.gameStatus)
                    {
                        case GameManager.GameStatus.Ingame:
                            if (inputManager.Tab)
                            {
                                OpenUI(GameManager.GameStatus.Ingame_Iventory);
                            }

                            if (inputManager.Q)
                            {
                                OpenUI(GameManager.GameStatus.Ingame_select_building_part);
                            }
                            break;

                        case GameManager.GameStatus.Ingame_Iventory:
                            if (inputManager.Tab || inputManager.Esc)
                            {
                                OpenUI(GameManager.GameStatus.Ingame);
                            }
                            break;

                        case GameManager.GameStatus.Ingame_select_building_part:
                            if (inputManager.Esc || inputManager.Q)
                            {
                                OpenUI(GameManager.GameStatus.Ingame);
                            }
                            break;

                        case GameManager.GameStatus.Ingame_placing_blueprints:
                            if (inputManager.Esc)
                            {
                                OpenUI(GameManager.GameStatus.Ingame);
                            }
                            if (inputManager.Q)
                            {
                                OpenUI(GameManager.GameStatus.Ingame_select_building_part);
                            }
                            break;

                        case GameManager.GameStatus.Ingame_Chest:
                            if (inputManager.Tab || inputManager.Esc)
                            {
                                OpenUI(GameManager.GameStatus.Ingame);
                            }
                            break;

                        default:
                            Debug.Log($"GameStatus: {GameManager.Instance.gameStatus} interaction is not set!");
                            OpenUI(GameManager.GameStatus.Ingame);
                            break;
                    }
                }
            }
        }

        private void OpenUI(GameManager.GameStatus setGameStatus)
        {
            var settings = settingsData.interfaceses;

            settings.timer = settings.interval;
            GameManager.Instance.gameStatus = setGameStatus;
        }
    }
}
