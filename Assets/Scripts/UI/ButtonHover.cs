using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHover : MonoBehaviour,IPointerExitHandler,IPointerEnterHandler,IPointerClickHandler
{
    public GameObject menu;

    public void OnPointerClick(PointerEventData eventData)
    {
        menu.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<Image>().color = new Color(1, 1, 1);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<Image>().color = new Color(0.75f, 0.75f, 0.75f);
    }
}
