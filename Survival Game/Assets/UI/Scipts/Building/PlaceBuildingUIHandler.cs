using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.Player;
using Game.Player.Controller;

namespace Game.UI.Building
{
    public class PlaceBuildingUIHandler : MonoBehaviour
    {
        [SerializeField] Image snapImage;
        [SerializeField] Color disabledColor;
        [SerializeField] Color activatedColor;

        private bool canChangeSnap;

        // Start is called before the first frame update
        void Start()
        {
            UpdateColor();
            canChangeSnap = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (canChangeSnap == true)
            {
                if (PlayerController.Instance.SnapIsPressed())
                {
                    UpdateColor();
                }
            }
            else
            {
                if (!PlayerController.Instance.SnapIsPressed())
                {
                    canChangeSnap = true;
                }
            }
        }

        private void UpdateColor()
        {
            if(snapImage != null && PlayerSettings.Instance != null)
            {
                PlayerSettings.Instance.buildingPartSnapping = !PlayerSettings.Instance.buildingPartSnapping;
                snapImage.color = PlayerSettings.Instance.buildingPartSnapping ? activatedColor : disabledColor;
                canChangeSnap = false;
            }
        }

    }
}
