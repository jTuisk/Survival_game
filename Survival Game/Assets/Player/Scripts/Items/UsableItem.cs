using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Game.Player.Item
{
    [System.Serializable]
    public class UsableItem : Item
    {
        public UnityEvent effect;

        public void UseItem()
        {
            effect.Invoke();
        }
    }
}