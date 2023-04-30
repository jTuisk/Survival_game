using System.Collections.Generic;
using UnityEngine;
using Game.Player;
using Game.Player.Inventory;

namespace Game.UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        public InventorySystem inventorySystem;

        public int defaultGroupIndex;
        private UI_Group activeGroup;
        public UI_Group[] uiGroups;
        public UI_Group ActiveGroup => activeGroup;

        private void Awake()
        {
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

        public void SetContainers(int chestIndex)
        {
            if(inventorySystem != null)
            {
                if (activeGroup.inventoryGroup.activeToolbar != null)
                {
                    activeGroup.inventoryGroup.activeToolbar.SetNewContainer(inventorySystem.ToolBarContainer);
                }

                if (activeGroup.inventoryGroup.activeBackpack != null)
                {
                    activeGroup.inventoryGroup.activeBackpack.SetNewContainer(inventorySystem.BackpackContainer);
                }

                if (activeGroup.inventoryGroup.activeChest != null)
                {
                    activeGroup.inventoryGroup.activeChest.SetNewContainer(inventorySystem.GetChestContainer(chestIndex));
                }
            }
            UpdateSlots();
        }

        public void ChangeUI(int groupIndex, int chestIndex = -1)
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
                SetContainers(chestIndex);
            }
            else
            {
                if (!activeGroup.Equals(uiGroups[defaultGroupIndex]))
                {
                    ChangeUI(defaultGroupIndex);
                }
            }
        }

        public void UpdateSlots()
        {

            if (activeGroup.inventoryGroup.activeToolbar != null)
            {
                activeGroup.inventoryGroup.activeToolbar.UpdateSlots();
            }

            if (activeGroup.inventoryGroup.activeBackpack != null)
            {
                activeGroup.inventoryGroup.activeBackpack.UpdateSlots();
            }

            if (activeGroup.inventoryGroup.activeChest != null)
            {
                activeGroup.inventoryGroup.activeChest.UpdateSlots();
            }
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
            public UI_InventorySystemHandler activeToolbar;
            public UI_InventorySystemHandler activeBackpack;
            public UI_InventorySystemHandler activeChest;
        }
    }
}
