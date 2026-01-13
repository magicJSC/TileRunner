using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EventHandler : MonoBehaviour, IPointerDownHandler
{
    public Action clickAction;

    public void OnPointerDown(PointerEventData eventData)
    {
        clickAction?.Invoke();
    }
}
