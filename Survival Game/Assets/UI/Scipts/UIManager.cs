using System.Collections.Generic;
using UnityEngine;
using Game.Player;
using Game.Player.Inventory;

namespace Game.UI
{
    public class UIManager : MonoBehaviour
    {
        public bool useCustomUI = false;
        public static UIManager Instance { get; private set; }

        public Canvas canvas;

        public InventorySystem inventorySystem;

        public int defaultGroupIndex;

        private UI_Group activeGroup;

        public List<GameObject> allUIElements;

        public UI_Group[] uiGroups;
        public UI_Group ActiveGroup => activeGroup;


        [SerializeField] UI_InventorySystemHandler toolbar;
        [SerializeField] UI_InventorySystemHandler backpack;
        [SerializeField] UI_InventorySystemHandler chest;

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
            InitInventoryContainers();
        }

        private void LateUpdate()
        {
            UpdateUIComparedToGameStatus();
        }

        private void InitInventoryContainers()
        {
            if (inventorySystem != null)
            {
                if (toolbar != null)
                {
                    toolbar.SetNewContainer(inventorySystem.ToolBarContainer);
                }

                if (backpack != null)
                {
                    backpack.SetNewContainer(inventorySystem.BackpackContainer);
                }
                UpdateSlots();
            }
        }

        public void UpdateSlots()
        {
            if (toolbar != null)
            {
                toolbar.UpdateSlots();
            }

            if (backpack != null)
            {
                backpack.UpdateSlots();
            }
            if (chest != null)
            {
                chest.UpdateSlots();
            }
        }

        public void SetActiveChest(int newChestIndex)
        {
            if (chest != null)
            {
                chest.SetNewContainer(inventorySystem.GetChestContainer(newChestIndex));
                chest.UpdateSlots();
            }
        }

        private void UpdateUIComparedToGameStatus()
        {

            if (!useCustomUI && GameManager.Instance != null)
            {
                if (activeGroup != null && activeGroup.status == GameManager.Instance.gameStatus)
                    return;

                //Debug.Log($"active group {activeGroup?.name}, status: {activeGroup?.status}/{GameManager.Instance.gameStatus}");

                foreach (UI_Group group in uiGroups)
                {
                    if(group.status == GameManager.Instance.gameStatus)
                    {
                        ChangeUI(group);
                        break;
                    }
                }
            }
        }


        private void ChangeUI(UI_Group toGroup)
        {
            if (activeGroup == null || !activeGroup.Equals(toGroup))
            {
                activeGroup = toGroup;

                PlayerSettings.Instance.canMove = !activeGroup.lockMovement;
                PlayerSettings.Instance.canRotateCamera = !activeGroup.lockView;
                HideCursor.ShowCurors(activeGroup.showCuror, activeGroup.lockCursor);

                allUIElements.ForEach(go => go.SetActive(false));
                activeGroup?.visible.ForEach(go => go.SetActive(true));
            }
            else
            {
                ChangeUI(uiGroups[0]);
            }
        }

        [System.Serializable]
        public class UI_Group
        {
            public GameManager.GameStatus status;
            public string name;
            public bool showCuror = false;
            public bool lockCursor = true;
            public bool lockView = false;
            public bool lockMovement = false;

            public List<GameObject> visible;

            public override bool Equals(object obj)
            {
                UI_Group other = obj as UI_Group;
                return other.name.Equals(name);
            }
        }
    }
}
