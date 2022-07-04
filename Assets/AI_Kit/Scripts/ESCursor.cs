using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESCursor : MonoBehaviour
{
    [HideInInspector]
    public bool ShowCursor = false;
    [HideInInspector]
    public bool lockcursor = true;

    private void Update()
    {
        _Cursor();
    }
    public void _Cursor()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            lockcursor = !lockcursor;
            ShowCursor = !ShowCursor;
        }
        Hide_ShowCursor(lockcursor, ShowCursor);
    }
    public static void Hide_ShowCursor(bool lockstate, bool visibility)
    {
        if (lockstate)
        {
            Cursor.lockState = CursorLockMode.Locked;

        }
        if (!lockstate)
        {
            Cursor.lockState = CursorLockMode.None;

        }
        //
        if (visibility)
        {
            Cursor.visible = true;
        }
        if (!visibility)
        {
            Cursor.visible = false;
        }
    }
    //

}
