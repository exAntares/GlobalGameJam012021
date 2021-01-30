using System;
using System.Collections.Generic;
using HalfBlind.ScriptableVariables;
using UnityEngine;
using Random = UnityEngine.Random;

public class TransformSpawner : MonoBehaviour {
    [Serializable]
    public class SpawnData {
        public int Weight = 1;
        public Transform ToSpawn;
    }
    
    [SerializeField] private SpawnData[] _toSpawn;
    [SerializeField] private Transform[] _spots;
    [SerializeField] private GlobalFloat _currentTurn;
    [SerializeField] private int _everyXTurns = 1;
    [SerializeField] private bool _shouldSpawnRotatedRandom = false;
    
    private void Awake() {
        _currentTurn.OnTValueChanged += OnTurnChanged;
    }

    private void OnDestroy() {
        _currentTurn.OnTValueChanged -= OnTurnChanged;
    }

    private void OnTurnChanged(float turn) {
        if (((int)turn % _everyXTurns) != 0) {
            return;
        }
        
        var weightedList = new List<SpawnData>();
        for (var i = 0; i < _toSpawn.Length; i++) {
            var spawnData = _toSpawn[i];
            for (int j = 0; j < spawnData.Weight; j++) {
                weightedList.Add(spawnData);
            }
        }
        
        var indexToSpawn = Random.Range(0, weightedList.Count);
        var spotIndex = Random.Range(0, _spots.Length);
        var instance = Instantiate(weightedList[indexToSpawn].ToSpawn);
        instance.position = _spots[spotIndex].position;
        if (_shouldSpawnRotatedRandom) {
            instance.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(-40, 40)));
        }
    }
}
