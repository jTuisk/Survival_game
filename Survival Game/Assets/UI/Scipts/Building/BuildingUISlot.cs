using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Game.UI.Building
{
    public class BuildingUISlot : MonoBehaviour, IPointerClickHandler
    {
        private Image backgroundImage;
        public Sprite defaultBackgroundImage;
        public Sprite selectedBackgroundImage;
        public Color defaultBackgroundColor;
        public Color selectedBackgroundColor;

        public GameObject buildingPart;

        private bool isSelected;
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                UpdateBackgroundImage();
            }
        }

        private void Awake()
        {
            backgroundImage = GetComponent<Image>();
            backgroundImage.alphaHitTestMinimumThreshold = 0.5f;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            //Hide layout, display selected item prefab
            //Show building layout

            CreateBuildingPart();
        }

        private void UpdateBackgroundImage()
        {
            backgroundImage.sprite = isSelected ? selectedBackgroundImage : defaultBackgroundImage;
            backgroundImage.color = isSelected ? selectedBackgroundColor : defaultBackgroundColor;
        }

        private void CreateBuildingPart()
        {
            GameManager.Instance.gameStatus = GameManager.GameStatus.Ingame_placing_blueprints;
            Debug.Log($"Building {buildingPart?.name}");
            Instantiate(buildingPart, transform.root);
        }
    }

}
