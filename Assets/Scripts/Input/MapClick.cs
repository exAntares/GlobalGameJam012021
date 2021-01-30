using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class MapClick : MonoBehaviour, IPointerClickHandler
{

    public ClickEvent mapClickedEvent;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked: " + eventData.pointerCurrentRaycast.gameObject.name);
        mapClickedEvent?.Invoke(eventData.pointerCurrentRaycast.worldPosition);
    }

    [Serializable]
    public class ClickEvent : UnityEvent<Vector3> 
    {

    }
}