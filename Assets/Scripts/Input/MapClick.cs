using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class MapClick : MonoBehaviour
{
    public ClickEvent mapClickedEvent;
    public Camera mainCamera;
    private int layer_mask;
    void Start() {
       layer_mask = LayerMask.GetMask("ScreenRaycast");
    }
    void Update()
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer_mask )) {
            mapClickedEvent?.Invoke(hit.point);
        }
    }

    [Serializable]
    public class ClickEvent : UnityEvent<Vector3> 
    {

    }
}