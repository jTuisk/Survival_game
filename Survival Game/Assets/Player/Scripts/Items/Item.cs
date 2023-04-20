using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player.Item
{
    [System.Serializable]
    public class Item : IItem
    {
        public bool canInteract = true;
        public GameObject prefab = null;
        public int id = -1;
        public string name = "N/A";
        public string description = "N/A";
        public float weight = 0f;
        public float value = 0f;
        public int maxStackAmount = 1;
        /*public ItemType Type { get { return Type; } protected set { Type = value; } }
        public int Id { get { return Id; } protected set { Id = value; } }
        public string Name { get { return Name; } protected set { Name = value; } }
        public string Description { get { return Description; } protected set { Description = value; } }
        public float Weight { get { return Weight; } protected set { Weight = value; } }
        public int CurrentStackAmount { get { return CurrentStackAmount; } protected set { CurrentStackAmount = value; } }
        public int MaxStackAmount { get { return MaxStackAmount; } protected set { MaxStackAmount = value; } }
        public float Value { get { return Value; } protected set { Value = value; } }*/

        public bool CanInteract()
        {
            return canInteract;
        }

        public void DropItemToGround(Transform ground)
        {
            throw new System.NotImplementedException();
        }

        public void PickUpItem()
        {
            throw new System.NotImplementedException();
        }

        public bool Stackable()
        {
            return maxStackAmount > 1;
        }

    }
}

