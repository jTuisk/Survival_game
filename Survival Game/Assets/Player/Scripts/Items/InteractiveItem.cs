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

        public GameObject quantityCanvas;
        public TextMeshProUGUI quantityText;

        private void Update()
        {
            UpdateQuantityText();
            quantityCanvas.transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
        }

        public bool Pickup()
        {
            if (itemData != null)
            {
                if (itemData.canInteract)
                {
                    if(InventorySystem.Instance.AddItem(this, quantity))
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
            if (quantityCanvas == null || quantityText == null)
                return;

            if (quantity > 0)
            {
                quantityCanvas.SetActive(true);
                quantityText.text = quantity.ToString();
            }
            else
            {
                quantityCanvas.SetActive(false);
                quantityText.text = "";
            }
        }
    }
}

