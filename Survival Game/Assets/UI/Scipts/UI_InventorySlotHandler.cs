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

        [SerializeField] Image icon;
        [SerializeField] TextMeshProUGUI tmpText;

        public void Start()
        {
            UpdateText(0);
        }

        public void UpdateData(InventorySlot inventorySlot)
        {
            UpdateIcon(inventorySlot.slotItem.item.icon);
            UpdateText(inventorySlot.itemQuantity);
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
