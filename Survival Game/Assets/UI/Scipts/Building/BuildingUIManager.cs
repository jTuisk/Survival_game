using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Game.UI.Building
{
    public class BuildingUIManager : MonoBehaviour
    {
        public static BuildingUIManager Instance;

        public List<BuildingUISlot> parts;

        private EventSystem eventSystem;

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

        private void Update()
        {
            var raycastResults = new List<RaycastResult>();
            var pointerEventData = new PointerEventData(eventSystem);
            pointerEventData.position = Input.mousePosition;
            EventSystem.current.RaycastAll(pointerEventData, raycastResults);
            if (raycastResults.Count > 0)
            {
                foreach (var result in raycastResults)
                {
                    BuildingUISlot selectedSlot;
                    if (result.gameObject.TryGetComponent<BuildingUISlot>(out selectedSlot))
                    {
                        SelectSlot(selectedSlot);
                        break;
                    }
                }
            }
        }


        public void SelectSlot(BuildingUISlot activeSlot)
        {
            foreach(var slot in parts)
            {
                slot.IsSelected = slot.Equals(activeSlot);
            }
        }
    }

}
