using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class MainMenu : MonoBehaviour
{
    private UIDocument _document;

    private Button _button;

    private void Awake()
    {
        _document = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        var root = _document.rootVisualElement;
        _button = root.Q<Button>("ExitMenuButton");

        if (_button != null)
        {
            _button.RegisterCallback<ClickEvent>(ExitMenuClick);
        }
    }

    private void OnDisable()
    {
        if (_button != null)
        {
            _button.UnregisterCallback<ClickEvent>(ExitMenuClick);
        }
    }

    private void ExitMenuClick(ClickEvent evt)
    {
        gameObject.SetActive(false);
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
