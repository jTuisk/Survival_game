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

        /*
        public void Init()
        {
            foreach (Transform child in transform)
            {
                GameObject.Destroy(child.gameObject);
            }

            //inventorySystem.InitUIContainerData();

            slots = new Dictionary<UI_InventorySlotHandler, InventorySlot>();

            var containerSlots = inventoryContainer.GetSlots();

            for (int i = 0; i < containerSlots.Length; i++)
            {
                GameObject go = Instantiate(UI_slotsPrefab, transform);
                slots.Add(go.GetComponent<UI_InventorySlotHandler>(), containerSlots[i]);
            }
        }

        public void UpdateSlot(InventorySlot slot, int n)
        {
            var uiSlot = slots.ElementAt(n).Key;

            if(uiSlot != null)
            {
                uiSlot.UpdateData(slot);
            }
        }

        public void UpdateSlots()
        {
            foreach(var slot in slots)
            {
                if(slot.Value != null)
                {
                    slot.Key.UpdateData(slot.Value);
                }
            }
        }

        public void UpdateSlot(InventorySlot slot)
        {
            Debug.Log(slots.Count);
            foreach (var s in slots)
            {
                Debug.Log($"{s.Key} - {s.Value} - {s.Value.Equals(slot)}");
            }
            var uiSlot = slots.FirstOrDefault(x => x.Equals(slot)).Key;
            if (uiSlot != null)
            {
                uiSlot.UpdateData(slot);
            }
        }*/
    }
}
