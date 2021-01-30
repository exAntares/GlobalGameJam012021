using DG.Tweening;
using HalfBlind.ScriptableVariables;
using Tooltips;
using TurnManagement;
using UnityEngine;

namespace Ship
{
    [RequireComponent(typeof(ShipPieceComponent)), RequireComponent(typeof(TooltipSpawner))]
    public class CannonPieceComponent : MonoBehaviour
    {
        [SerializeField] private ShipPieceComponent _shipPiece;
        [SerializeField] private TooltipSpawner _tooltipSpawner;
        [SerializeField] private string _tooltipTextWhenDetached;
        [SerializeField] private string _tooltipTextWhenAttached;
        [SerializeField] private ScriptableGameEvent _onTurnPassedEvent;
        [SerializeField] private GameObject _projectile;
        [SerializeField] private GameObject _hitEffect;
        [SerializeField] private GlobalFloat _globalCannonDamageInt;
        
        private void Reset()
        {
            _shipPiece = GetComponent<ShipPieceComponent>();
            _tooltipSpawner = GetComponent<TooltipSpawner>();
        }

        private void Start()
        {
            if (_shipPiece == null || _tooltipSpawner == null)
            {
                Reset();
            }
            
            if (_shipPiece.IsAttached)
            {
                OnAttached();
            }
            else
            {
                OnDetached();
            }
        }

        private void Awake()
        {
            AddListeners();
        }

        private void OnDestroy()
        {
            RemoveListeners();
        }

        private void RemoveListeners()
        {
            _onTurnPassedEvent.RemoveListener(OnTurnPassed);
            _shipPiece.OnAttachedEvent.RemoveListener(OnAttached);
            _shipPiece.OnDetachedEvent.RemoveListener(OnDetached);
            _shipPiece.OnClickedEvent.RemoveListener(OnClicked);
        }

        private void AddListeners()
        {
            _onTurnPassedEvent.AddListener(OnTurnPassed);
            _shipPiece.OnAttachedEvent.AddListener(OnAttached);
            _shipPiece.OnDetachedEvent.AddListener(OnDetached);
            _shipPiece.OnClickedEvent.AddListener(OnClicked);
        }

        private void OnClicked() {
            var enemies = FindObjectsOfType<EnemyComponent>();
            var enemiesLength = enemies.Length;
            if (enemiesLength > 0) {
                var allCannons = FindObjectsOfType<CannonPieceComponent>();
                foreach (var cannonPiece in allCannons) {
                    if (cannonPiece._shipPiece.IsAttached) {
                        cannonPiece.ShootAtRandom(enemies);
                    }
                }

                var turnManager = TurnManager.Find();
                turnManager.PassTurn();
            }
        }

        private void ShootAtRandom(EnemyComponent[] enemies)
        {
            var enemiesLength = enemies.Length;
            var range = Random.Range(0, enemiesLength);
            var enemy = enemies[range];

            var instance = Instantiate(_projectile);
            instance.transform.position = transform.position;
            var tween = instance.transform.DOMove(enemy.transform.position, 0.5f);
            tween.SetDelay(Random.Range(0f, 0.5f));
            tween.OnComplete(() => {
                var hitEffect = Instantiate(_hitEffect);
                hitEffect.transform.position = instance.transform.position;
                Destroy(hitEffect, 2.0f);
                Destroy(instance);
                if (enemy != null && enemy.enabled)
                {
                    var damage = (int) Mathf.Max(_globalCannonDamageInt.Value, 1f);
                    enemy.ReceiveDamage(damage);
                }
            });
        }

        private void OnTurnPassed()
        {
            RefreshText();
        }

        private void RefreshText()
        {
            if (_shipPiece.IsAttached)
            {
                _tooltipSpawner.Text = GetTextForEnemies();
            }
            else
            {
                _tooltipSpawner.Text = _tooltipTextWhenDetached;
            }
        }

        private string GetTextForEnemies()
        {
            var enemies = FindObjectsOfType<EnemyComponent>();
            var enemiesLength = enemies.Length;
            if (enemiesLength > 0)
            {
                var allCannons = FindObjectsOfType<CannonPieceComponent>();
                return $"Your {allCannons.Length} cannons will attack {enemiesLength} enemies";
            }

            return _tooltipTextWhenAttached;
        }

        private void OnDetached()
        {
            RefreshText();
        }

        private void OnAttached()
        {
            RefreshText();
        }
    }
}