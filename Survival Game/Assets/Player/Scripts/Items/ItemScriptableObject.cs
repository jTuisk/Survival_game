using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player.Item
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Items/New Item")]
    public class ItemScriptableObject : ScriptableObject
    {
        public enum ItemType { Other, Weapon, Armor, Food, Drink, Potion }
        public ItemType itemType = ItemType.Other;

        [ConditionalHide("itemType", 0)]
        public Item item;

        [ConditionalHide("itemType", new int[] {1, 2})]
        public WearableItem wearableItem;

        [ConditionalHide("itemType", new int[] {3, 4, 5})]
        public UsableItem usableItem;
    }
}