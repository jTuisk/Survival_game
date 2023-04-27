using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Player.Items;


namespace Game.Player.Inventory
{
    [System.Serializable]
    public class InventoryContainer
    {

        public bool dropOverQuantityItems = true;
        [SerializeField, ReadOnly] private uint inventorySize;
        [SerializeField] private InventorySlot[] slots;

        public InventoryContainer(uint size)
        {
            inventorySize = size;
            InitializeSlots();
        }

        private void InitializeSlots()
        {

            slots = new InventorySlot[inventorySize];
            for (int i = 0; i < slots.Length; i++)
            {
                slots[i] = new InventorySlot();
            }
        }

        #region ADD


        public void ForceAddItem(ItemScriptableObject item, int quantity = 1)
        {
            slots[GetEmptySlotIndex()].SetItem(item, quantity);
        }

        public void ForceAddItem(InventorySlot slot, ItemScriptableObject item, int quantity = 1)
        {
            slots[GetSlotIndex(slot)].SetItem(item, quantity);
        }

        public bool AddItem(ItemScriptableObject item, int quantity = 1)
        {
            int startQuantity = quantity;
            int[] slotsIndexs = GetItemSlotIndexs(item).ToArray();
            int[] addAmount = new int[slotsIndexs.Length];

            string debugText = "";
            for (int i = 0; i < slotsIndexs.Length; i++)
            {
                addAmount[i] = slots[slotsIndexs[i]].PreCheckAdd();
                quantity -= addAmount[i];
                debugText += $"i: {i}, slot[{slotsIndexs[i]}], amount: {addAmount[i]}, left: {quantity} \n";
            }
            
            Debug.Log($"{debugText} \n final quantity: {quantity}");

            if(quantity > 0)
            {
                int[] emptySlotIndexs = GetEmptySlotIndexs();

                bool enoughRoom = (emptySlotIndexs.Length * item.item.maxStackAmount) >= quantity;

                if (enoughRoom)
                {
                    for (int i = 0; i < addAmount.Length; i++)
                    {
                        slots[i].AddQuantity(addAmount[i]);
                    }

                    for (int i = 0; i < emptySlotIndexs.Length; i++)
                    {
                        if (quantity <= 0)
                            break;

                        int newSlotQuantity = Mathf.Min(quantity, item.item.maxStackAmount);
                        slots[emptySlotIndexs[i]].SetItem(item, newSlotQuantity);
                        quantity -= newSlotQuantity;
                    }
                    return true;
                }
                else
                {
                    //Display not enough room warning
                    //play sounds?
                    Debug.Log($"Not enought room to pickup {startQuantity} * {item.item.name}");
                    return false;
                }
            }
            else if(quantity == 0)
            {
                for (int i = 0; i < addAmount.Length; i++)
                {
                    slots[i].AddQuantity(addAmount[i]);
                }
                return true;
            }
            return false;
        }

        #endregion


        #region REMOVE
        public void RemoveItem(int slotIndex)
        {
            slots[slotIndex].ClearSlot();
        }

        public void RemoveItem(InventorySlot slot)
        {
            RemoveItem(GetSlotIndex(slot));
        }

        public void RemoveItem(ItemScriptableObject item)
        {
            RemoveItem(GetItemSlotIndex(item));
        }

        public bool RemoveItem(ItemScriptableObject item, int amount)
        {

            int[] itemIndexs = GetItemSlotIndexs(item).ToArray();
            int[] removeAmount = new int[itemIndexs.Length];

            for(int i = 0; i < itemIndexs.Length; i++)
            {
                removeAmount[i] = slots[itemIndexs[i]].PreCheckRemove(amount);
                amount -= removeAmount[i];
            }

            if(amount == 0)
            {
                for (int i = 0; i < removeAmount.Length; i++)
                {
                    slots[i].RemoveItem(removeAmount[i]);
                }
                return true;
            }
            return false;
        }
        #endregion


        #region MOVE
        public void MoveItemToSlot(InventorySlot slot, InventorySlot toSlot)
        {
            MoveItemToSlot(GetSlotIndex(slot), GetSlotIndex(toSlot));
        }

        public void MoveItemToSlot(int index, int toIndex)
        {
            InventorySlot tempSlot = slots[toIndex];
            slots[toIndex] = slots[index];
            slots[index] = tempSlot;
        }
        #endregion


        #region GETTERS
        public int GetSlotIndex(InventorySlot slot)
        {
            for(int i = 0; i < slots.Length; i++)
            {
                if (slots[i].Equals(slot))
                    return i;
            }
            return -1;
        }

        public int GetItemSlotIndex(ItemScriptableObject item)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].slotItem.Equals(item))
                {
                    return i;
                }
            }
            return -1;
        }

        public List<int> GetItemSlotIndexs(ItemScriptableObject item)
        {
            List<int> itemIndexs = new List<int>();

            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].slotItem != null && slots[i].slotItem.Equals(item))
                {
                    itemIndexs.Add(i);
                }
            }
            return itemIndexs;
        }

        public int GetEmptySlotIndex()
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].slotItem == null)
                    return i;
            }
            return -1;
        }
        public int[] GetEmptySlotIndexs()
        {
            List<int> indexs = new List<int>();
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].slotItem == null)
                    indexs.Add(i);
            }

            return indexs.ToArray();
        }
    }

    #endregion

}

