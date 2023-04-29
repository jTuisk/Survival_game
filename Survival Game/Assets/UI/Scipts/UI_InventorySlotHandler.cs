using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Player.Inventory;

namespace Game.UI
{
    public class UI_InventorySlotHandler : MonoBehaviour
    {
        private Sprite defaultSprite;
        [SerializeField] Image icon;
        [SerializeField] TextMeshProUGUI tmpText;

        private void Awake()
        {
            defaultSprite = icon.sprite;
        }

        private void Start()
        {
            UpdateText(0);
        }

        public void UpdateData(InventorySlot inventorySlot)
        {
            if(inventorySlot != null)
            {
                if(inventorySlot.slotItem != null)
                    UpdateIcon(inventorySlot.slotItem.item.icon);

                UpdateText(inventorySlot.itemQuantity);
            }
            else
            {
                UpdateIcon(defaultSprite);
                UpdateText(0);
            }
        }

        public void UpdateIcon(Sprite newSprite)
        {
            icon.sprite = newSprite;
        }

        public void UpdateText(int quantity)
        {
            if(quantity > 1)
            {
                tmpText.text = quantity.ToString();
            }
            else
            {
                tmpText.text = "";
            }
        }

    }

}
