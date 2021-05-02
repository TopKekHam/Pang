using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonPressedEventListner : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public UnityEvent OnDown, OnUp;

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDown.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnUp.Invoke();
    }
}

