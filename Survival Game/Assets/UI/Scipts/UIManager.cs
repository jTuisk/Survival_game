using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public UI_Group[] uiGroups;


    [System.Serializable]
    public class UI_Group
    {
        public List<GameObject> visible;
        public List<GameObject> disabled;
    }

}
