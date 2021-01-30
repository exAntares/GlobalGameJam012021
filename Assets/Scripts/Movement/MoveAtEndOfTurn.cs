using DG.Tweening;
using UnityEngine;

namespace Movement
{
    public class MoveAtEndOfTurn : MonoBehaviour
    {
        public Vector2 DirectionPerTurn;
        [SerializeField] private ShipPieceComponent _attachedToShipPiece;
        [SerializeField] private float _destroyWhenBelowY = -5f;

        private void Start()
        {
            if (_attachedToShipPiece == null)
            {
                _attachedToShipPiece = GetComponent<ShipPieceComponent>();
            }

            if (_attachedToShipPiece != null)
            {
                _attachedToShipPiece.OnAttachedEvent.AddListener(OnAttached);
                _attachedToShipPiece.OnDetachedEvent.AddListener(OnDetached);
            }
        }

        private void Reset() => _attachedToShipPiece = GetComponent<ShipPieceComponent>();

        private void OnDetached() => enabled = true;

        private void OnAttached() => enabled = false;

        public void DoMovement()
        {
            if (!enabled)
            {
                return;
            }

            if (!IsOutsideScreen())
            {
                transform.DOMove(transform.position + new Vector3(DirectionPerTurn.x, DirectionPerTurn.y, 0), 0.3f).SetAutoKill(true);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public bool IsOutsideScreen() => transform.position.y < _destroyWhenBelowY;
    }
}