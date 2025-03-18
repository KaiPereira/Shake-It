using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChangeCursor : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    public Texture2D customCursor;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.SetCursor(customCursor, Vector2.zero, CursorMode.Auto);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
