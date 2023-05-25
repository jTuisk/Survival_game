using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.UI;

namespace Game.Player.Inventory
{
    public class ChestHandler : MonoBehaviour
    {
        bool chestCreated = false;

        [SerializeField] int chestId;
        [SerializeField] uint chestSize;
        InventoryContainer container;

        // Update is called once per frame
        void Start()
        {
            CreateChest();
        }


        void CreateChest()
        {
            if (!chestCreated && InventorySystem.Instance != null)
            { //InventoryContainer CreateNewChestContainer(uint size)
                container = InventorySystem.Instance.CreateNewChestContainer(chestSize);
                chestId = InventorySystem.Instance.chestContainers.Count - 1;
                chestCreated = true;
                Debug.Log($"Chest({chestId}) is created!");
            }
        }

        public void OpenChest()
        {
            if(chestCreated && chestId > -1)
            {
                Debug.Log($"Opening chest({chestId})!");
                UIManager.Instance.SetActiveChest(chestId);
                GameManager.Instance.gameStatus = GameManager.GameStatus.Ingame_Chest;
            }
        }
    }

}
