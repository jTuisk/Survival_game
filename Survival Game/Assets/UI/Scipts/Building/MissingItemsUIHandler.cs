using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Game.UI.Building
{
    public class MissingItemsUIHandler : MonoBehaviour
    {
        public Image spriteImage;
        public TextMeshProUGUI quantityText;

        public void UpdateData(Sprite toSprite, int quantity)
        {
            UpdateImageSprite(toSprite);
            UpdateQuantityText(quantity);
        }

        public void UpdateQuantityText(int quantity)
        {
            quantityText.text = quantity.ToString();
        }

        public void UpdateImageSprite(Sprite toSprite)
        {

            if (spriteImage != null)
            {
                spriteImage.sprite = toSprite;
            }
        }
    }

}
