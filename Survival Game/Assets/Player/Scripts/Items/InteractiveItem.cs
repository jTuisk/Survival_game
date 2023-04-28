using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Player.Inventory;
using TMPro;

namespace Game.Player.Items
{
    public class InteractiveItem : MonoBehaviour
    {
        public ItemScriptableObject itemData;
        public int quantity = 1;

        public GameObject quantityTextOverlay;
        public TextMeshProUGUI quantityText;

        public bool Pickup(InventorySystem inventory)
        {
            if (itemData != null)
            {
                if (itemData.canInteract)
                {
                    if(inventory.AddItem(this, quantity))
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

        public int RemoveQuantity(int n)
        {
            int fillAmount = Mathf.Min(quantity, n);
            quantity -= fillAmount;
            UpdateQuantityText();

            return fillAmount;
        }

        public void AddQuantity(int n)
        {
            quantity += n;
            UpdateQuantityText();
        }

        private void UpdateQuantityText()
        {
            if(quantity > 1)
            {
                quantityTextOverlay.SetActive(true);
                quantityText.text = quantity.ToString();
            }
            else
            {
                quantityTextOverlay.SetActive(false);
                quantityText.text = "";
            }
        }
    }
}

