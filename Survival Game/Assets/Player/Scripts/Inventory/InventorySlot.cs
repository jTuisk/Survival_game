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

        public InventorySlot(ItemScriptableObject slotItem = null, int itemQuantity = -1)
        {
            this.slotItem = slotItem;
            this.itemQuantity = itemQuantity;
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
            int newQuantity = (itemQuantity + amount);
            if(newQuantity <= slotItem.item.maxStackAmount)
            {
                return amount;
            }
            else
            {
                return amount - (newQuantity - slotItem.item.maxStackAmount);
            }
        }

        public void AddQuantity(int add)
        {
            int newQuantity = itemQuantity + add;
            int maxStack = slotItem.item.maxStackAmount;
            if (newQuantity <= maxStack)
            {
                itemQuantity = newQuantity;
            }
            else
            {
                itemQuantity = maxStack;
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