using System.Collections.Generic;
using UnityEngine;

public class MainShipComponent : MonoBehaviour {
    public const int maxX = 100;
    public const int maxY = 100;
    public readonly Vector3 Offset = new Vector3(Mathf.FloorToInt(maxX * 0.5f), Mathf.FloorToInt(maxY * 0.5f));
    private Dictionary<int, ShipPieceComponent> _piecesByIndex = new Dictionary<int, ShipPieceComponent>();

    private void Awake() => AddPiece(GetComponentInChildren<ShipPieceComponent>());

    public int GetIndex(Vector3 localPos) {
        var offsetPos = localPos + Offset;
        return Mathf.RoundToInt(offsetPos.y * maxX + offsetPos.x);
    }
    
    public void AddPiece(ShipPieceComponent piece) {
        if (_piecesByIndex.Count <= 0) {
            piece.transform.SetParent(transform);
            piece.transform.localPosition = Vector3.zero;
            var index = GetIndex(Vector3.zero);
            _piecesByIndex[index] = piece;
            piece.OnAttached();
            return;
        }

        var availableIndex = GetAvailableIndex();
        if (availableIndex > 0) {
            piece.transform.SetParent(transform);
            piece.transform.localPosition = GetLocalPos(availableIndex);
            _piecesByIndex[availableIndex] = piece;
            piece.OnAttached();
        }
    }

    public Vector3 GetLocalPos(int index) => new Vector3(index % maxX, Mathf.FloorToInt(index / (float) maxX)) - Offset;

    public int GetAvailableIndex() {
        foreach (var pieces in _piecesByIndex) {
            var position = pieces.Value.transform.localPosition;
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
        return !_piecesByIndex.TryGetValue(index, out var shipPiece) || shipPiece == null;
    }
}
