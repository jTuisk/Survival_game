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

        public bool displayQuantityText = true;
        public GameObject quantityCanvas;
        public TextMeshProUGUI quantityText;


        private void Update()
        {

            quantityCanvas.SetActive(displayQuantityText);
            if (displayQuantityText)
            {
                UpdateQuantityText();
                quantityCanvas.transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
            }
        }

        public bool Pickup()
        {
            if (itemData != null)
            {
                if (itemData.canInteract)
                {
                    InventorySystem.Instance.AddItem(this, ref quantity);
                    UpdateQuantityText();
                    if (quantity < 1)
                    {
                        Destroy(gameObject);
                    }
                    return true;
                }
                else
                {
                    Debug.Log($"Item: {itemData.itemData.id} - {itemData.itemData.name}, interaction is disabled!");
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

            quantityCanvas.SetActive(displayQuantityText);

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

