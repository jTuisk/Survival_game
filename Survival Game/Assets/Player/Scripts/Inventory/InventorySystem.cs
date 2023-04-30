using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Player.Items;
using Game.UI;

namespace Game.Player.Inventory
{
    [ExecuteInEditMode]
    public class InventorySystem : MonoBehaviour
    {
        public StartItem[] startingItems;

        public Transform planetItemsParent;

        public static InventorySystem Instance { get; private set; }
        public uint toolbarContainerSize = 7;
        public uint backpackContainerSize = 24;

        public InventoryContainer toolbarContainer;
        public InventoryContainer backpackContainer;
        public List<InventoryContainer> chestContainers;

        public InventoryContainer ToolBarContainer => toolbarContainer;
        public InventoryContainer BackpackContainer => backpackContainer;

        public void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }

            toolbarContainer = new InventoryContainer(toolbarContainerSize);
            backpackContainer = new InventoryContainer(backpackContainerSize);
            chestContainers = new List<InventoryContainer>();

            //Remove after testing
            AddStartingItems();
        }

        //Remove after testing
        private void AddStartingItems()
        {
            for(int i = 0; i < startingItems.Length; i++)
            {
                toolbarContainer.ForceSetItem(i, startingItems[i].item.GetComponent<InteractiveItem>().itemData, startingItems[i].amount);
                backpackContainer.ForceSetItem(i, startingItems[i].item.GetComponent<InteractiveItem>().itemData, startingItems[i].amount);
            }
        }

        public void SwapItemSlots(InventorySlot from, InventorySlot to)
        {
            if (from == null || to == null)
                return;

            InventorySlot tempSlot = new InventorySlot(to.item, to.itemQuantity);
            to.SetItem(from.item, from.itemQuantity);
            from.SetItem(tempSlot.item, tempSlot.itemQuantity);
            UIManager.Instance?.UpdateSlots();
        }

        public void SplitItemBetweenSlots(InventorySlot from, InventorySlot to)
        {
            if(from.item != null && to.item == null)
            {
                int halfQuantity = (int)Mathf.Floor(from.itemQuantity / 2);
                Debug.Log($"half: {halfQuantity}");
                if(halfQuantity > 0)
                {
                    from.SetQuantity(from.itemQuantity - halfQuantity);
                    to.SetItem(from.item, halfQuantity);
                }
                else
                {
                    SwapItemSlots(from, to);
                }
            }
        }

        public void CombineItemSlots(InventorySlot from, InventorySlot to)
        {
            if (from != null && from.item.Equals(to.item))
            {
                to.AddQuantity(ref from.itemQuantity);
                if(from.itemQuantity <= 0)
                {
                    from.ClearSlot();
                }
            }
            else
            {
                SwapItemSlots(from, to);
            }
        }

        public void DropItem(InventorySlot item, int quantity = -1)
        {
            //if quantity equals -1, then drop everything.
            item.DropItemToGround(quantity);
        }

        public void AddItem(InteractiveItem item, ref int quantity)
        {
            if(!toolbarContainer.AddItem(item, ref quantity))
            {
                backpackContainer.AddItem(item, ref quantity);
            }
        }

        public InventoryContainer GetChestContainer(int index)
        {
            return chestContainers[index];
        }

        public InventoryContainer CreateNewChestContainer(uint size)
        {
            chestContainers.Add(new InventoryContainer(size));
            return chestContainers[chestContainers.Count-1];
        }

        public bool AddItemToChest(int chestIndex, InteractiveItem item, ref int quantity)
        {
            return chestContainers[chestIndex].AddItem(item, ref quantity);
        }

        //Remove after testing
        [System.Serializable]
        public class StartItem
        {
            public GameObject item;
            public int amount;
        }
    }
}
