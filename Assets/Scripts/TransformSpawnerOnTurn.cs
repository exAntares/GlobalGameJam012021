using HalfBlind.ScriptableVariables;
using UnityEngine;
using Random = UnityEngine.Random;

public class TransformSpawnerOnTurn : MonoBehaviour {
    
    [SerializeField] private Transform[] _toSpawn;
    [SerializeField] private Transform[] _spots;
    [SerializeField] private GlobalFloat _currentTurn;
    [SerializeField] private GlobalFloat _maxTurnTurn;
    [SerializeField] private int _lastTurnOffset = 3;
    [SerializeField] private bool _shouldSpawnRotatedRandom = false;
    
    private void Awake() {
        _currentTurn.OnTValueChanged += OnTurnChanged;
    }

    private void OnDestroy() {
        _currentTurn.OnTValueChanged -= OnTurnChanged;
    }

    private void OnTurnChanged(float turn) {
        var currentTurn = (int)turn;
        var maxTurn = (int) _maxTurnTurn.Value;
        
        if (currentTurn != maxTurn - _lastTurnOffset) {
            return;
        }
        
        var indexToSpawn = Random.Range(0, _toSpawn.Length);
        var spotIndex = Random.Range(0, _spots.Length);
        var instance = Instantiate(_toSpawn[indexToSpawn]);
        instance.position = _spots[spotIndex].position;
        if (_shouldSpawnRotatedRandom) {
            instance.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(-40, 40)));
        }
    }
}
