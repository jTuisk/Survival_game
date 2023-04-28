using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player.Controller
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Controller/Interaction Settings")]
    public class InteractionSettingsScriptableObject : ScriptableObject
    {
        public ItemPickup itemPickup;
        public TabMenu tabMenu;

        [System.Serializable]
        public class ItemPickup
        {
            public bool canPickup = true;
            public float interval = 0.5f;
            public float timer = 0f;
            public float distance = 1f;
            public LayerMask itemLayer;
        }

        [System.Serializable]
        public class TabMenu
        {
            public bool canOpen = true;
            public float interval = 0.1f;
            public float timer = 0f;
        }
    }
}
