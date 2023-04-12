using UnityEngine;

public class HideCursor : MonoBehaviour
{

    public static void ShowCurors(bool showCursor, bool lockCursor)
    {
        Cursor.visible = showCursor;
        Cursor.lockState = lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
