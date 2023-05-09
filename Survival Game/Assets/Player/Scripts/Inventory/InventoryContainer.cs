using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Player.Items;
using Game.UI;


namespace Game.Player.Inventory
{
    [System.Serializable]
    public class InventoryContainer
    {
        [SerializeField] private InventorySlot[] slots;

        public InventorySlot[] GetSlots() => slots;

        public InventoryContainer(uint size)
        {
            InitializeSlots(size);
        }

        private void InitializeSlots(uint size)
        {

            slots = new InventorySlot[size];
            for (int i = 0; i < slots.Length; i++)
            {
                slots[i] = new InventorySlot();
            }
        }

        #region ADD

        public void ForceSetItem(int slotIndex, ItemScriptableObject item, int quantity)
        {
            slots[slotIndex].SetItem(item, quantity);
        }

        public bool AddItem(InteractiveItem item, ref int quantity)
        {
            ItemScriptableObject itemData = item.itemData;
            int startQuantity = quantity;

            int[] slotsIndexs = GetItemSlotIndexs(itemData).ToArray();

            //Add item to slot where the same item already exists
            for (int i = 0; i < slotsIndexs.Length; i++) 
            {
                if (quantity <= 0)
                    return true;

                slots[slotsIndexs[i]].AddQuantity(ref quantity);
            }

            //Add item to new empty slot
            if (quantity > 0)
            {
                int[] emptySlotIndexs = GetEmptySlotIndexs();

                for (int i = 0; i < emptySlotIndexs.Length; i++)
                {
                    if (quantity <= 0)
                        return true;

                    slots[emptySlotIndexs[i]].SetItem(itemData);
                    slots[emptySlotIndexs[i]].AddQuantity(ref quantity);
                }
            }
            else
            {
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

        public bool RemoveQuantity(ItemScriptableObject item, ref int quantity)
        {
            List<int> slotsIndexs = GetItemSlotIndexs(item);
            int startQuantity = quantity;

            for (int i = 0; i < slotsIndexs.Count; i++)
            {
                if (quantity <= 0)
                    return true;

                slots[slotsIndexs[i]].RemoveQuantity(ref quantity);
            }
            return false;
        }

        public bool eqwe(InteractiveItem item, ref int quantity)
        {
            ItemScriptableObject itemData = item.itemData;
            int startQuantity = quantity;

            int[] slotsIndexs = GetItemSlotIndexs(itemData).ToArray();

            //Add item to slot where the same item already exists
            for (int i = 0; i < slotsIndexs.Length; i++)
            {
                if (quantity <= 0)
                    return true;

                slots[slotsIndexs[i]].AddQuantity(ref quantity);
            }

            //Add item to new empty slot
            if (quantity > 0)
            {
                int[] emptySlotIndexs = GetEmptySlotIndexs();

                for (int i = 0; i < emptySlotIndexs.Length; i++)
                {
                    if (quantity <= 0)
                        return true;

                    slots[emptySlotIndexs[i]].SetItem(itemData);
                    slots[emptySlotIndexs[i]].AddQuantity(ref quantity);
                }
            }
            else
            {
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

        public int GetItemAmount(ItemScriptableObject item)
        {
            int totalAmount = 0;
            List<int> itemIndexs = GetItemSlotIndexs(item);

            for(int i = 0; i < itemIndexs.Count; i++)
            {
                totalAmount += slots[itemIndexs[i]].itemQuantity;
            }
            return totalAmount;
        }

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
                if (slots[i].item.Equals(item))
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
                if (slots[i].item != null && slots[i].item.Equals(item))
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
                if (slots[i].item == null)
                    return i;
            }
            return -1;
        }
        public int[] GetEmptySlotIndexs()
        {
            List<int> indexs = new List<int>();
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item == null)
                    indexs.Add(i);
            }

            return indexs.ToArray();
        }
    }

    #endregion

}

