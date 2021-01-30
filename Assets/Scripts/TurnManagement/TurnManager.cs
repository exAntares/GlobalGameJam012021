using HalfBlind.ScriptableVariables;
using Movement;
using UnityEngine;
using UnityEngine.Events;

namespace TurnManagement
{
    public class TurnManager : MonoBehaviour
    {
        private static TurnManager _turnManager;
        [SerializeField] private ScriptableGameEvent _eventToSendOnTurnEnd;
        [SerializeField] private GlobalFloat _currentTurnGlobalFloat;
        private IPerformTurnAction _playerActionToPerform;

        public UnityEvent<int> OnTurnPassed;
        public UnityEvent<string> OnTurnPassedAsString;

        private int _currentTurn;

        public int CurrentTurn => _currentTurn;

        public static TurnManager Find()
        {
            if (_turnManager == null)
            {
                _turnManager = FindObjectOfType<TurnManager>();
            }

            return _turnManager;
        }

        public void SetPlayerAction(IPerformTurnAction performTurnAction)
        {
            _playerActionToPerform = performTurnAction;
        }

        public void PassTurn()
        {
            OnTurnPassed.Invoke(++_currentTurn);
            OnTurnPassedAsString.Invoke(_currentTurn.ToString());
            _currentTurnGlobalFloat.Value = _currentTurn;
            if (_playerActionToPerform != null)
            {
                _playerActionToPerform.DoTurnAction();
                _playerActionToPerform = null;
            }

            var moveAtEndOfTurns = FindObjectsOfType<MoveAtEndOfTurn>();
            foreach (var moveAtEndOfTurn in moveAtEndOfTurns)
            {
                moveAtEndOfTurn.DoMovement();
            }
            _eventToSendOnTurnEnd.SendEvent();
        }
    }
}