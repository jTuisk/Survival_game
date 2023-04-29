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
        }

        public void AddItem(InteractiveItem item, ref int quantity)
        {
            if(!toolbarContainer.AddItem(item, ref quantity)) //this ref doesn't update quantity value..
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
    }
}
