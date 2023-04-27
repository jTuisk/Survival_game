using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Player.Inventory;


namespace Game.Player.Items
{
    public class InteractiveItem : MonoBehaviour
    {
        public ItemScriptableObject itemData;
        public int quantity = 1;

        public bool Pickup(InventoryContainer playerInventory)
        {
            if (itemData != null)
            {
                if (itemData.canInteract)
                {
                    if(playerInventory.AddItem(itemData, quantity))
                    {
                        //Play sound
                        //Special "smoke style" particles around object that was pickedup.
                        Destroy(gameObject);
                        return true;
                    }
                }
                else
                {
                    Debug.Log($"Item: {itemData.item.id} - {itemData.item.name}, interaction is disabled!");
                }
            }
            else
            {
                Debug.Log($"{gameObject.name} itemData is not set!");
            }
            return false;
        }
    }
}

