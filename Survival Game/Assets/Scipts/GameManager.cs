using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public GameStatus gameStatus;

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
        public enum GameStatus { MainMenu, PauseMenu, EndMenu, Ingame, Ingame_building, Ingame_Crafting, Ingame_Iventory }
    }

}

