using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Game.Misc;
using Game.Player;

namespace Game.UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        public int defaultGroupIndex;
        private UI_Group activeGroup;
        public UI_Group[] uiGroups;
        public UI_Group ActiveGroup => activeGroup;

        private void Awake()
        {
            // If there is an instance, and it's not me, delete myself.

            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        private void Start()
        {
            ChangeUI(defaultGroupIndex);
        }

        public void ChangeUI(int groupIndex)
        {
            groupIndex = groupIndex > uiGroups.Length ? uiGroups.Length - 1 : groupIndex;

            UI_Group uiGroup = uiGroups[groupIndex];

            if (activeGroup == null || !activeGroup.Equals(uiGroup))
            {
                activeGroup = uiGroup;
                HideCursor.ShowCurors(activeGroup.showCuror, activeGroup.lockCursor);
                PlayerSettings.Instance.canMove = !activeGroup.lockMovement;
                PlayerSettings.Instance.canRotateCamera = !activeGroup.lockView;

                activeGroup?.visible.ForEach(go => go.SetActive(true));
                activeGroup?.disabled.ForEach(go => go.SetActive(false));
                //TODO:
                    //Set inventoryGroup data
                    //Refresh inventory slots!
            }
            else
            {
                if (!activeGroup.Equals(uiGroups[defaultGroupIndex]))
                {
                    ChangeUI(defaultGroupIndex);
                }
            }
        }

        public void ChangeUI(string name) 
        {
            UI_Group group = uiGroups.First(x => x.Equals(name));
            ChangeUI(0); //Redo this to get group index
        }



        [System.Serializable]
        public class UI_Group
        {
            public string name;
            public bool showCuror = false;
            public bool lockCursor = true;
            public bool lockView = false;
            public bool lockMovement = false;

            public List<GameObject> visible;
            public List<GameObject> disabled;

            public UI_InventoryGroup inventoryGroup;


            public override bool Equals(object obj)
            {
                UI_Group other = obj as UI_Group;
                return other.name.Equals(name);
            }
        }

        [System.Serializable]
        public class UI_InventoryGroup
        {
            public UI_InventorySystemHandler activeToolBar;
            public UI_InventorySystemHandler activeBackpack;
            public UI_InventorySystemHandler activeChest;
        }
    }
}
