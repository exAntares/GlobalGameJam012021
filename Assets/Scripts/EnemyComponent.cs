using System.Linq;
using DG.Tweening;
using HalfBlind.ScriptableVariables;
using Ship;
using UnityEngine;

public class EnemyComponent : MonoBehaviour {
    [SerializeField] private GameObject _projectile;
    [SerializeField] private GameObject _hitEffect;
    [SerializeField] private GlobalFloat _currentTurnGlobalFloat;
    [SerializeField] private int _turnFrequencyToAttack = 3;
    [SerializeField] private int _hitPoints = 3;
    [SerializeField] private ScriptableGameEvent _sendWhenKilled;
    [SerializeField] private ScriptableGameEvent _sendWhenDestroyBoat;
    [SerializeField] private ScriptableGameEvent _sendWhenHit;

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
        var shipPieceComponents = mainShip.PiecesByIndex.Values.Where(x => x != null).ToList();
        var targetRaftComponent = shipPieceComponents
            .FirstOrDefault(x => x.GetComponent<TargetRaftComponent>() != null);
        if (targetRaftComponent != null) {
            DestroyShip(targetRaftComponent.gameObject);
            return;
        }

        shipPieceComponents = shipPieceComponents.Where(x => x.transform.localPosition != Vector3.zero).ToList();
        if (shipPieceComponents.Count > 0) {
            var index = Random.Range(0, shipPieceComponents.Count);
            var target = shipPieceComponents[index].gameObject;
            if (target != null)
            {
                Debug.Log($"{name} attacking {target}");
                DestroyShip(target);
            }            
        }
    }

    public void ReceiveDamage(int damage)
    {
        _hitPoints -= damage;
        if (_hitPoints < 0)
        {
            var allComponents = gameObject.GetComponents<MonoBehaviour>();
            foreach (var monoBehaviour in allComponents)
            {
                // Stop moving or accepting things from this enemy
                monoBehaviour.enabled = false;
            }

            transform.DOScale(Vector3.zero, 0.3f).OnComplete(() =>
            {
                if (gameObject != null)
                {
                    if (_sendWhenKilled != null)
                    {
                        _sendWhenKilled.SendEvent();
                    }
                    Destroy(gameObject);
                }
            });
        }
        else
        {
            if (_sendWhenHit != null)
            {
                _sendWhenHit.SendEvent();
            }
        }
    }

    private void DestroyShip(GameObject target) {
        var instance = Instantiate(_projectile);
        instance.transform.position = transform.position;
        var tweenerCore = instance.transform.DOMove(target.transform.position, 0.5f);
        tweenerCore.SetDelay(Random.Range(0f, 0.5f));
        tweenerCore.OnComplete(() => {
            var hitEffect = Instantiate(_hitEffect);
            hitEffect.transform.position = instance.transform.position;
            Destroy(hitEffect, 2.0f);
            Destroy(instance);
            if (target != null)
            {
                if (_sendWhenDestroyBoat != null)
                {
                    _sendWhenDestroyBoat.SendEvent();
                }

                Destroy(target);
            }
        });
    }
}
