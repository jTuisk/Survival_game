using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Player.Items;
using Game.UI;

namespace Game.Player.Inventory
{
    [ExecuteInEditMode]
    public class PlayerInventorySystem : InventorySystem
    {
        [Header("Backpack")]
        public uint backpackSize = 24;
        public InventoryContainer backpackContainer;
        public UI_InventorySystemHandler backpackUIHandler;

        public override void Awake()
        {
            primaryContainer = new InventoryContainer(primaryContainerSize, primaryUIHandler);
            backpackContainer = new InventoryContainer(backpackSize, backpackUIHandler);
        }

        public override bool AddItem(InteractiveItem item, int quantity = 1)
        {
            if(!primaryContainer.AddItem(item, quantity))
            {
                return backpackContainer.AddItem(item, quantity);
            }
            return true;
        }

        public override void InitUIContainerData()
        {
            Debug.Log($"{primaryContainer}, {backpackContainer}");
            if(primaryContainer != null)
            {
                primaryUIHandler.inventoryContainer = primaryContainer;
            }

            if(backpackContainer != null)
            {
                backpackUIHandler.inventoryContainer = backpackContainer;
            }
        }
    }
}
