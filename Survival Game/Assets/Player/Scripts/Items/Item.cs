using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player.Items
{
    [System.Serializable]
    public class Item
    {
        public GameObject prefab = null;
        public Sprite icon;

        [Header("Data")]
        public int id = -1;
        public string name = "N/A";
        [TextArea(2,2)]
        public string description = "N/A";
        public float weight = 0f;
        public float value = 0f;
        public int maxStackAmount = 1;

        public bool Stackable()
        {
            return maxStackAmount > 1;
        }

        public override bool Equals(object obj)
        {
            Item other = obj as Item;
            return other.id == this.id;
        }
    }
}

