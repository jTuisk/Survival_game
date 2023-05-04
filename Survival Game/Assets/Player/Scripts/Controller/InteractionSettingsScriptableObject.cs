using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player.Controller
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Controller/Interaction Settings")]
    public class InteractionSettingsScriptableObject : ScriptableObject
    {
        public ItemPickup itemPickup;
        public Interfaceses interfaceses;
        public Building building;

        [System.Serializable]
        public class ItemPickup
        {
            public bool canPickup = true;
            public float cooldown = 0.5f;
            public float failCooldown = 0.4f;
            public float timer = 0f;
            public float distance = 1f;
            public LayerMask itemLayer;
        }

        [System.Serializable]
        public class Interfaceses
        {
            public bool canOpen = true;
            public float interval = 0.1f;
            public float timer = 0f;
        }

        [System.Serializable]
        public class Building
        {
            public float interval = 0.5f;
            public float timer = 0f;
            public float distance = 1f;
            public LayerMask itemLayer;
        }
    }
}
