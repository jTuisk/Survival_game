using UnityEngine;

public class HideCursor : MonoBehaviour
{
    [SerializeField] bool lockCursor = true;
    [SerializeField] bool hideCursor = true;
    // Update is called once per frame
    void Update()
    {
        Cursor.visible = !hideCursor;
        Cursor.lockState = lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
