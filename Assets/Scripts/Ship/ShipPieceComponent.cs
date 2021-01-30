using UnityEngine;
using UnityEngine.EventSystems;

public class ShipPieceComponent : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler {
    protected bool IsAttached = false;
    
    private void OnDestroy() {
        OnDeattached();
    }

    public virtual void OnClicked() {
        Debug.Log($"{name} OnClicked");
        if (!IsAttached) {
            var mainShip = FindObjectOfType<MainShipComponent>();
            mainShip.AddPiece(this);            
        }
    }

    public virtual void OnAttached() {
        Debug.Log($"{name} OnAttached");
        IsAttached = true;
    }

    public virtual void OnDeattached() {
        Debug.Log($"{name} OnDeattached");
        IsAttached = false;
    }

    public void OnPointerClick(PointerEventData eventData) {
        OnClicked();
    }

    public void OnPointerDown(PointerEventData eventData) {
    }

    public void OnPointerUp(PointerEventData eventData) {
    }
}