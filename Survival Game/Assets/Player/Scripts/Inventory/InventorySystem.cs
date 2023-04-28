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
        public uint primaryContainerSize = 1;
        public InventoryContainer primaryContainer;
        public UI_InventorySystemHandler primaryUIHandler;

        public virtual void Awake()
        {
            primaryContainer = new InventoryContainer(primaryContainerSize, primaryUIHandler);
        }

        public virtual bool AddItem(InteractiveItem item, int quantity = 1)
        {
            return primaryContainer.AddItem(item, quantity);
        }

        public virtual void InitUIContainerData()
        {
            if (primaryContainer != null)
            {
                primaryUIHandler.inventoryContainer = primaryContainer;
            }
        }
    }
}
