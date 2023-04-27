using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Player.Items;


namespace Game.Player.Inventory
{
    [System.Serializable]
    public class InventorySlot
    {
        public ItemScriptableObject slotItem;
        public int itemQuantity;
        public TextMeshProUGUI slotText;
        public TextMeshProUGUI quantityText;
        public Image slotImage;

        public InventorySlot(ItemScriptableObject slotItem = null, int itemQuantity = -1)
        {
            this.slotItem = slotItem;
            this.itemQuantity = itemQuantity;
        }

        public void UpdateSlotUI()
        {
            UpdateText();
            UpdateQuantityText();
            UpdateImage();
        }

        public void UpdateText()
        {
            if (slotItem != null)
            {
                slotText.text = slotItem.item.name;
            }
            else
            {
                slotText.text = "";
            }
            
        }
        public void UpdateQuantityText()
        {
            if(slotItem != null && slotItem.item.maxStackAmount > 1)
            {
                quantityText.enabled = true;
                quantityText.text = itemQuantity.ToString();
            }
            else
            {
                quantityText.enabled = false;
            }
        }

        public void UpdateImage()
        {
            if (slotItem != null && slotItem.item.icon != null)
            {
                slotImage.enabled = true;
                slotImage.sprite = slotItem.item.icon;
            }
            else
            {
                slotImage.enabled = false;
            }
        }

        public void SetItem(ItemScriptableObject item, int quantity = 1)
        {
            slotItem = item;
            itemQuantity = quantity;
        }

        public void ClearSlot()
        {
            SetItem(null, -1);
        }

        public int PreCheckAdd(int amount = 0)
        {
            return Mathf.Max(0, slotItem.item.maxStackAmount - (itemQuantity + amount));
        }

        public int AddQuantity(int add)
        {
            int newQuantity = itemQuantity + add;
            int maxStack = slotItem.item.maxStackAmount;
            if (newQuantity <= maxStack)
            {
                itemQuantity = newQuantity;
                return 0;
            }
            else
            {
                itemQuantity = maxStack;
                return maxStack - newQuantity;
            }
        }

        public int PreCheckRemove(int amount)
        {
            int newQuantity = amount - itemQuantity;
            return newQuantity < 0 ? newQuantity + itemQuantity : newQuantity;
        }

        public void RemoveItem(int remove)
        {
            int newQuantity = itemQuantity - remove;

            if(newQuantity > 0)
            {
                itemQuantity = newQuantity;
            }
            else
            {
                ClearSlot();
            }
        }
    }
}