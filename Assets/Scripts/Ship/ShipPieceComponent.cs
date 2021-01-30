using TurnManagement;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShipPieceComponent : MonoBehaviour, IPerformTurnAction, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler {
    protected bool IsAttached = false;
    
    private void OnDestroy() {
        OnDetached();
    }

    public virtual void OnClicked() {
        Debug.Log($"{name} OnClicked");
        if (!IsAttached)
        {
            var turnManager = TurnManager.Find();
            turnManager.SetPlayerAction(this);
        }
    }

    public virtual void OnAttached() {
        Debug.Log($"{name} OnAttached");
        IsAttached = true;
    }

    public virtual void OnDetached() {
        Debug.Log($"{name} OnDetached");
        IsAttached = false;
    }

    public void OnPointerClick(PointerEventData eventData) {
        OnClicked();
    }

    public void OnPointerDown(PointerEventData eventData) {
    }

    public void OnPointerUp(PointerEventData eventData) {
    }

    public void DoTurnAction()
    {
        var mainShip = FindObjectOfType<MainShipComponent>();
        mainShip.AddPiece(this);
    }
}