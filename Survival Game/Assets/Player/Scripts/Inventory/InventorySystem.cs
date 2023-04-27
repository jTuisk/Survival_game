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
        public uint inventorySize = 1;
        public InventoryContainer inventoryContainer;
        public UI_InventorySystemHandler inventorySystemHandler;

        public void Awake()
        {
            inventoryContainer = new InventoryContainer(inventorySize, inventorySystemHandler);
        }

        public bool AddToContainer(ItemScriptableObject item, int quantity = 1)
        {

            return false;
        }
    }
}
