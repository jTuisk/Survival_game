using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player.Item
{
    public interface IItem
    {
        public bool CanInteract();
        public void PickUpItem();
        public void DropItemToGround(Transform ground);
    }
}
