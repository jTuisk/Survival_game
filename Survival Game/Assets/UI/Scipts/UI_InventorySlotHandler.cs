using Game.Player.Inventory;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Game.Player.Controller;

namespace Game.UI
{
    public class UI_InventorySlotHandler : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private Sprite defaultSprite;
        [SerializeField] Image icon;
        [SerializeField] TextMeshProUGUI tmpText;

        public Image backgroundImage;

        public InventorySlot ActiveInventorySlot { get; private set; }

        //For Drag and drop
        public bool canDrag = true;
        private GameObject onDragItemCopy;
        private InventorySlot droppedOn;

        private void Awake()
        {
            defaultSprite = icon.sprite;
        }

        public void UpdateData(InventorySlot inventorySlot)
        {
            if(inventorySlot != null)
            {
                ActiveInventorySlot = inventorySlot;

                UpdateIcon(inventorySlot);
                UpdateText(inventorySlot);
            }
            else
            {
                UpdateIcon(defaultSprite);
                UpdateText("");
            }
        }

        public void UpdateData()
        {
            UpdateData(ActiveInventorySlot);
        }

        private void UpdateIcon(Sprite newSprite)
        {
            icon.sprite = newSprite;
        }

        private void UpdateIcon(InventorySlot slot = null)
        {
            if (slot != null)
            {
                ActiveInventorySlot = slot;
            }

            if (ActiveInventorySlot != null)
            {
                if (ActiveInventorySlot.item != null && ActiveInventorySlot.itemQuantity > 0)
                {
                    icon.sprite = ActiveInventorySlot.item.itemData.icon;
                }
                else
                {
                    icon.sprite = defaultSprite;
                }
            }
        }

        private void UpdateText(InventorySlot slot = null)
        {
            if(slot != null)
            {
                ActiveInventorySlot = slot;
            }

            if(ActiveInventorySlot != null)
            {
                if(ActiveInventorySlot.item != null && ActiveInventorySlot.item.itemData.maxStackAmount > 1)
                {
                    tmpText.text = ActiveInventorySlot.itemQuantity.ToString();
                }
                else
                {
                    tmpText.text = "";
                }
            }
        }

        private void UpdateText(string quantity)
        {
            tmpText.text = quantity.ToString();
        }

        public void SetBackgroundVisibility(float n)
        {
            backgroundImage.color = new Color(backgroundImage.color.r, backgroundImage.color.g, backgroundImage.color.b, n);
        }

        public void SetIconVisibility(float n)
        {
            icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, n);
        }

        public void CreateCopy()
        {
            onDragItemCopy = Instantiate(gameObject, UIManager.Instance.canvas.transform);
            onDragItemCopy.GetComponent<RectTransform>().sizeDelta = gameObject.GetComponent<RectTransform>().sizeDelta;

            UI_InventorySlotHandler uiHandler = onDragItemCopy.GetComponent<UI_InventorySlotHandler>();
            uiHandler.canDrag = false;
            uiHandler.icon.raycastTarget = false;
            uiHandler.SetBackgroundVisibility(0f);

            SetIconVisibility(0f);
            UpdateText("");
        }

        public void DestroyCopy()
        {
            Destroy(onDragItemCopy);
            SetIconVisibility(255f);
            UpdateText();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (canDrag && ActiveInventorySlot.item != null)
            {
                CreateCopy();
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if(onDragItemCopy != null)
            {
                onDragItemCopy.transform.position = eventData.position;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (onDragItemCopy != null)
            {
                DestroyCopy();

                var raycastResults = new List<RaycastResult>();
                EventSystem.current.RaycastAll(eventData, raycastResults);

                if (raycastResults.Count > 0)
                {
                    bool foundSlotData = false;
                    UI_InventorySlotHandler onSlot;
                    foreach (var result in raycastResults)
                    {
                        if (result.gameObject.transform.parent.TryGetComponent<UI_InventorySlotHandler>(out onSlot))
                        {
                            if(onSlot != null && !onSlot.Equals(this))
                            {
                                droppedOn = onSlot.ActiveInventorySlot;

                                if (!PlayerController.Instance.ShiftIsPressed())
                                {
                                    InventorySystem.Instance.SwapItemSlots(ActiveInventorySlot, droppedOn);
                                }
                                else
                                {
                                    if (droppedOn.item == null) //Split 
                                    {
                                        InventorySystem.Instance.SplitItemBetweenSlots(ActiveInventorySlot, droppedOn);
                                    }
                                    else // fill the slot item got dropped on
                                    {
                                        InventorySystem.Instance.CombineItemSlots(ActiveInventorySlot, droppedOn);
                                    }
                                }
                                foundSlotData = true;
                                break;
                            }
                            else if(onSlot.Equals(this))
                            {
                                foundSlotData = true;
                            }
                        }
                    }
                    //Drop item to the ground
                    if (!foundSlotData)
                    {
                        InventorySystem.Instance.DropItem(ActiveInventorySlot);
                    }
                }
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log($"Click item slot: {gameObject.name}, item: {ActiveInventorySlot?.item?.itemData.name}, quantity: {ActiveInventorySlot?.itemQuantity}");
        }
    }
}
