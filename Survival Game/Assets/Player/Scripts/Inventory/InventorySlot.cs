using UnityEngine;
using Game.Player.Items;
using Game.Player.Controller;
using Game.UI;


namespace Game.Player.Inventory
{
    [System.Serializable]
    public class InventorySlot
    {
        public ItemScriptableObject item;
        public int itemQuantity;

        public InventorySlot(ItemScriptableObject slotItem = null, int itemQuantity = -1)
        {
            this.item = slotItem;
            this.itemQuantity = itemQuantity;
        }

        public void SetItem(ItemScriptableObject item, int quantity = 0)
        {
            this.item = item;
            itemQuantity = quantity;
            UIManager.Instance?.UpdateSlots();
        }

        public void SetQuantity(int newQuantity)
        {
            itemQuantity = newQuantity;
            if(itemQuantity < 1)
            {
                item = null;
            }
            UIManager.Instance?.UpdateSlots();
        }

        public void RemoveQuantity(ref int amount)
        {
            int finalQuantity = itemQuantity - amount;
            int removedAmount = Mathf.Max(finalQuantity, 0);
            removedAmount = Mathf.Min(removedAmount, amount);
            itemQuantity -= removedAmount;
            amount -= removedAmount;
            UIManager.Instance?.UpdateSlots();
        }

        public void ClearSlot()
        {
            SetItem(null, -1);
            UIManager.Instance?.UpdateSlots();
        }

        public void AddQuantity(ref int amount)
        {
            int quantityLeft = item.itemData.maxStackAmount - itemQuantity;
            int addAmount = Mathf.Min(quantityLeft, amount);
            addAmount = Mathf.Max(addAmount, 0);
            itemQuantity += addAmount;
            amount -= addAmount;
            UIManager.Instance?.UpdateSlots();
        }

        public void DropItemToGround(int quantity)
        {
            if (quantity == -1)
            {
                quantity = itemQuantity;
            }
            else
            {
                quantity = Mathf.Min(quantity, itemQuantity);
            }

            if (quantity > 0)
            {
                GameObject groundItem = CreateItemGameObject();
                groundItem.GetComponent<InteractiveItem>().quantity = itemQuantity;
                groundItem.transform.position = PlayerController.Instance.GetPlayerLocation() + PlayerController.Instance.GetPlayerFoward()*0.5f;
                ClearSlot();
            }
        }

        private GameObject CreateItemGameObject()
        {
            return GameObject.Instantiate(item.itemData.prefab, InventorySystem.Instance.planetItemsParent);
        }
    }
}