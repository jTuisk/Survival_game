using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player.Items
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Items/New Item")]
    public class ItemScriptableObject : ScriptableObject
    {
        public enum ItemType { Other, Weapon, Armor, Food, Drink, Potion }
        public ItemType itemType = ItemType.Other;

        public bool canInteract = true;
        public Item itemData;
    }
}