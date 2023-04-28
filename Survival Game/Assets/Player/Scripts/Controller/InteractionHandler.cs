using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Player.Input;
using Game.Player.Items;
using Game.Player.Inventory;
using Game.UI;

namespace Game.Player.Controller
{
    [System.Serializable]
    public class InteractionHandler
    {
        private InputManager inputManager;
        private InteractionSettingsScriptableObject settingsData;
        private InventorySystem inventory;

        public InteractionHandler(InputManager inputManager, InteractionSettingsScriptableObject settings, InventorySystem inventory)
        {
            this.inputManager = inputManager;
            this.settingsData = settings;
            this.inventory = inventory;
        }

        public void Handle()
        {
            HandleItemPickup();
            HandleTabButton();
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

                        if(iItem != null)
                        {
                            Item item = iItem.itemData.item;
                            Debug.Log($"id: {item.id}, name:{item.name}");
                            if (iItem.Pickup(inventory))
                            {
                                settings.timer = settings.interval;
                                Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * settings.distance, Color.green);
                            }
                            else
                            {
                                settings.timer = settings.interval/10;
                                Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * settings.distance, Color.cyan);
                            }
                        }
                    }
                    else
                    {
                        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * settings.distance, Color.red);
                    }
                }
            }
        }

        private void HandleTabButton()
        {
            var settings = settingsData.tabMenu;
            if (settings.timer > 0)
            {
                settings.timer -= Time.deltaTime;
            }
            else
            {
                if (inputManager.Tab && settings.canOpen)
                {

                    UIManager.Instance.ChangeUI(1);
                    settings.timer = settings.interval;
                }
            }
        }
    }
}
