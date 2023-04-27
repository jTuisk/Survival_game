using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Player.Input;
using Game.Player.Items;
using Game.Player.Inventory;

namespace Game.Player.Controller
{
    [System.Serializable]
    public class InteractionHandler
    {
        private InputManager inputManager;
        private InteractionSettingsScriptableObject settings;
        private InventorySystem inventory;

        public InteractionHandler(InputManager inputManager, InteractionSettingsScriptableObject settings, InventorySystem inventory)
        {
            this.inputManager = inputManager;
            this.settings = settings;
            this.inventory = inventory;
        }

        public void Handle()
        {
            HandleItemPickup();
        }


        private void HandleItemPickup()
        {
            var itemPickup = settings.itemPickup;

            if (itemPickup.timer > 0f)
            {
                itemPickup.timer -= Time.deltaTime;
                Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * itemPickup.distance, Color.blue);
            }
            else
            {
                if (inputManager.Interact && itemPickup.canPickup)
                {
                    RaycastHit hit;

                    if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, itemPickup.distance, itemPickup.itemLayer.value))
                    {
                        InteractiveItem iItem = hit.transform.GetComponent<InteractiveItem>();

                        if(iItem != null)
                        {
                            Item item = iItem.itemData.item;
                            Debug.Log($"id: {item.id}, name:{item.name}");
                            if (iItem.Pickup(inventory.inventoryContainer))
                            {
                                itemPickup.timer = itemPickup.interval;
                                Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * itemPickup.distance, Color.green);
                            }
                            else
                            {
                                itemPickup.timer = itemPickup.interval/10;
                                Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * itemPickup.distance, Color.cyan);
                            }
                        }
                    }
                    else
                    {
                        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * itemPickup.distance, Color.red);
                    }
                }
            }
        }
    }
}
