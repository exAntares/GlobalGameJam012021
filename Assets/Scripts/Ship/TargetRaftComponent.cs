using Tooltips;
using UnityEngine;

namespace Ship {
    [RequireComponent(typeof(ShipPieceComponent)), RequireComponent(typeof(TooltipSpawner))]
    public class TargetRaftComponent : MonoBehaviour {
        [SerializeField] private ShipPieceComponent _shipPiece;
        [SerializeField] private TooltipSpawner _tooltipSpawner;
        [SerializeField] private string _tooltipTextWhenDetached;
        [SerializeField] private string _tooltipTextWhenAttached;

        private void Reset() {
            _shipPiece = GetComponent<ShipPieceComponent>();
            _tooltipSpawner = GetComponent<TooltipSpawner>();
        }

        private void Start() {
            if (_shipPiece == null || _tooltipSpawner == null) {
                Reset();
            }

            if (_shipPiece.IsAttached) {
                OnAttached();
            }
            else {
                OnDetached();
            }
        }

        private void OnEnable() {
            AddListeners();
        }

        private void OnDisable() {
            RemoveListeners();
        }

        private void RemoveListeners() {
            _shipPiece.OnAttachedEvent.RemoveListener(OnAttached);
            _shipPiece.OnDetachedEvent.RemoveListener(OnDetached);
            _shipPiece.OnClickedEvent.RemoveListener(OnClicked);
        }

        private void AddListeners() {
            _shipPiece.OnAttachedEvent.AddListener(OnAttached);
            _shipPiece.OnDetachedEvent.AddListener(OnDetached);
            _shipPiece.OnClickedEvent.AddListener(OnClicked);
        }

        private void OnClicked() { }

        private void OnDetached() {
            _tooltipSpawner.Text = _tooltipTextWhenDetached;
        }

        private void OnAttached() {
            _tooltipSpawner.Text = _tooltipTextWhenAttached;
        }
    }
}