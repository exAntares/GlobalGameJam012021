using TurnManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ShipPieceComponent : MonoBehaviour, IPerformTurnAction, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler {
    protected bool IsAttached = false;

    public UnityEvent OnAttachedEvent;
    public UnityEvent OnDetachedEvent;
    
    private void OnDestroy() {
        OnDetached();
    }

    public virtual void OnClicked() {
        Debug.Log($"{name} OnClicked");
        if (!IsAttached)
        {
            var turnManager = TurnManager.Find();
            turnManager.SetPlayerAction(this);
            turnManager.PassTurn();
        }
    }

    public virtual void OnAttached() {
        Debug.Log($"{name} OnAttached");
        OnAttachedEvent?.Invoke();
        IsAttached = true;
    }

    public virtual void OnDetached() {
        Debug.Log($"{name} OnDetached");
        OnDetachedEvent?.Invoke();
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