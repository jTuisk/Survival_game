using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Game.Player.Inventory;

namespace Game.UI
{
    public class UI_InventorySystemHandler : MonoBehaviour
    {
        public GameObject UI_slotsPrefab;
        [SerializeField] private InventoryContainer inventoryContainer;
        private Dictionary<UI_InventorySlotHandler, InventorySlot> uiSlots;

        public void SetNewContainer(InventoryContainer newContainer)
        {
            foreach (Transform child in transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            inventoryContainer = newContainer;
            uiSlots = new Dictionary<UI_InventorySlotHandler, InventorySlot>();

            var containerSlots = inventoryContainer.GetSlots();

            for(int i = 0; i < containerSlots.Length; i++)
            {
                GameObject go = Instantiate(UI_slotsPrefab, transform);
                uiSlots.Add(go.GetComponent<UI_InventorySlotHandler>(), containerSlots[i]);
            }
        }

        public void UpdateSlots()
        {
            foreach (var slot in uiSlots)
            {
                if (slot.Value != null)
                {
                    slot.Key.UpdateData(slot.Value);
                }
            }
        }
    }
}
