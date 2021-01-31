using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using HalfBlind.ScriptableVariables;
using Ship;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyComponent : MonoBehaviour {
    [SerializeField] private GameObject _projectile;
    [SerializeField] private GameObject _hitEffect;
    [SerializeField] private GlobalFloat _currentTurnGlobalFloat;
    [SerializeField] private int _turnFrequencyToAttack = 3;
    [SerializeField] private int _hitPoints = 3;
    [SerializeField] private ScriptableGameEvent _sendWhenKilled;
    [SerializeField] private ScriptableGameEvent _sendWhenDestroyBoat;
    [SerializeField] private ScriptableGameEvent _sendWhenHit;
    [SerializeField] private Animator _animator;
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Dead = Animator.StringToHash("Dead");

    public int HitPoints => _hitPoints;

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
            DestroyShip(targetRaftComponent.gameObject).Forget();
            return;
        }

        shipPieceComponents = shipPieceComponents.Where(x => x.transform.localPosition != Vector3.zero).ToList();
        if (shipPieceComponents.Count > 0) {
            var index = Random.Range(0, shipPieceComponents.Count);
            var target = shipPieceComponents[index].gameObject;
            if (target != null)
            {
                Debug.Log($"{name} attacking {target}");
                DestroyShip(target).Forget();
            }
        }
    }

    public void ReceiveDamage(int damage) {
        _hitPoints -= damage;
        if (_hitPoints == 0) {
            var allComponents = gameObject.GetComponents<MonoBehaviour>();
            foreach (var monoBehaviour in allComponents) {
                // Stop moving or accepting things from this enemy
                monoBehaviour.enabled = false;
            }

            PlayAnimationAndDie().Forget();
        }
        else if(_hitPoints > 0) {
            if (_sendWhenHit != null) {
                _sendWhenHit.SendEvent();
            }
        }
    }

    private async UniTaskVoid PlayAnimationAndDie() {
        const float animDuration = 0.45f;
        Debug.Log($"PlayAnimationAndDie {name} {Time.realtimeSinceStartup}");

        if (_animator != null) {
            _animator.SetTrigger(Dead);
        }
        else {
            transform.DOScale(Vector3.zero, animDuration);
        }
        
        // replacement of yield return new WaitForSeconds/WaitForSecondsRealtime
        await UniTask.Delay(TimeSpan.FromSeconds(animDuration))
            .WithCancellation(this.GetCancellationTokenOnDestroy());
        
        if (_sendWhenKilled != null) {
            _sendWhenKilled.SendEvent();
        }

        Destroy(gameObject);
    }

    private async UniTaskVoid DestroyShip(GameObject target) {
        if (_animator != null) {
            _animator.SetTrigger(Attack);
            Debug.Log($"DestroyShip SetTrigger {Time.realtimeSinceStartup}");

            // replacement of yield return new WaitForSeconds/WaitForSecondsRealtime
            await UniTask.Delay(TimeSpan.FromSeconds(1.2f))
                .WithCancellation(this.GetCancellationTokenOnDestroy());            
        }

        Debug.Log($"DestroyShip Instantiate {Time.realtimeSinceStartup}");
        if (target != null) {
            var instance = Instantiate(_projectile);
            instance.transform.position = transform.position;
            var tweenerCore = instance.transform.DOMove(target.transform.position, 0.5f);
            if (_animator == null) {
                tweenerCore.SetDelay(Random.Range(0.1f, 0.3f));
            }
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
}
