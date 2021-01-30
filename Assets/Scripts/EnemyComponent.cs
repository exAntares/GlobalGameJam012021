using System.Linq;
using DG.Tweening;
using HalfBlind.ScriptableVariables;
using UnityEngine;

public class EnemyComponent : MonoBehaviour {
    [SerializeField] private GameObject _projectile;
    [SerializeField] private GlobalFloat _currentTurnGlobalFloat;
    [SerializeField] private int _turnFrequencyToAttack = 3;
    [SerializeField] private int _hitPoints = 3;

    private void Start() {
        _currentTurnGlobalFloat.OnTValueChanged += OnTurnChanged;
    }

    private void OnDestroy() {
        _currentTurnGlobalFloat.OnTValueChanged -= OnTurnChanged;
    }

    private void OnTurnChanged(float turn) {
        var intTurn = (int)turn;
        var mod = intTurn % _turnFrequencyToAttack;
        if (mod == 0) {
            AttackPlayer();
        }
    }

    private void AttackPlayer() {
        var mainShip = FindObjectOfType<MainShipComponent>();
        var shipPieceComponents = mainShip.PiecesByIndex.Values.ToArray();
        var index = Random.Range(0, shipPieceComponents.Length);
        var target = shipPieceComponents[index].gameObject;
        Debug.Log($"{name} attacking {target}");
        DestroyShip(target);
    }

    private void DestroyShip(GameObject target) {
        var instance = Instantiate(_projectile);
        instance.transform.position = transform.position;
        var tweenerCore = instance.transform.DOMove(target.transform.position, 0.5f);
        tweenerCore.OnComplete(action: () => {
            Destroy(instance);
            Destroy(target);
        });
    }
}
