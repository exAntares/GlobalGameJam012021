using System;
using HalfBlind.ScriptableVariables;
using UnityEngine;
using UnityEngine.Events;

namespace Sounds
{
    public class RunOnEvent : MonoBehaviour
    {
        [SerializeField] private ScriptableGameEvent _gameEvent;

        public UnityEvent OnEvent;

        private void OnEnable()
        {
            _gameEvent.AddListener(OnEventTriggered);
        }

        private void OnEventTriggered()
        {
            OnEvent?.Invoke();
        }

        private void OnDisable()
        {
            _gameEvent.RemoveListener(OnEventTriggered);
        }
    }
}