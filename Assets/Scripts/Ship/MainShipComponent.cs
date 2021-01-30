using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.U2D;

public class MainShipComponent : MonoBehaviour {
    public SpriteShapeController ShapeController;
    
    public const int maxX = 100;
    public const int maxY = 100;
    public readonly Vector3 Offset = new Vector3(Mathf.FloorToInt(maxX * 0.5f), Mathf.FloorToInt(maxY * 0.5f));
    public Dictionary<int, ShipPieceComponent> PiecesByIndex = new Dictionary<int, ShipPieceComponent>();
    private Transform _lastPiece;
    
    private void Awake() => AddPiece(GetComponentInChildren<ShipPieceComponent>());

    public int GetIndex(Vector3 localPos) {
        var offsetPos = localPos + Offset;
        return Mathf.RoundToInt(offsetPos.y * maxX + offsetPos.x);
    }
    
    public void AddPiece(ShipPieceComponent piece) {
        var pieceTransform = piece.transform;
        if (PiecesByIndex.Count <= 0) {
            pieceTransform.SetParent(transform);
            pieceTransform.localPosition = Vector3.zero;
            var index = GetIndex(Vector3.zero);
            PiecesByIndex[index] = piece;
            piece.OnAttached();
            return;
        }

        var availableIndex = GetAvailableIndex();
        if (availableIndex > 0) {
            pieceTransform.SetParent(transform);
            var targetPos = GetLocalPos(availableIndex);
            pieceTransform.DOLocalMove(targetPos, 0.5f);
            pieceTransform.DOLocalRotate(Vector3.zero, 0.5f);
            PiecesByIndex[availableIndex] = piece;
            _lastPiece = piece.transform;
            piece.OnAttached();
        }
    }

    public void Update() {
        if (_lastPiece != null) {
            ShapeController.spline.SetPosition(ShapeController.spline.GetPointCount()-1, _lastPiece.localPosition);
        }
    }

    private void OnCollisionEnter(Collision other) => Debug.Log($"{name} collision with {other.gameObject.name}");

    public Vector3 GetLocalPos(int index) => new Vector3(index % maxX, Mathf.FloorToInt(index / (float) maxX)) - Offset;

    public int GetAvailableIndex() {
        foreach (var piece in PiecesByIndex) {
            var position = GetLocalPos(piece.Key);
            var up = position + Vector3.up;
            if (IsEmpty(up)) {
                return GetIndex(up);
            }
            
            var left = position + Vector3.left;
            if (IsEmpty(left)) {
                return GetIndex(left);
            }
            
            var down = position + Vector3.down;
            if (IsEmpty(down)) {
                return GetIndex(down);
            }
            
            var right = position + Vector3.right;
            if (IsEmpty(right)) {
                return GetIndex(right);
            }
        }

        return -1;
    }

    public bool IsEmpty(Vector3 localPos) {
        var index = GetIndex(localPos);
        return !PiecesByIndex.TryGetValue(index, out var shipPiece) || shipPiece == null;
    }
}
