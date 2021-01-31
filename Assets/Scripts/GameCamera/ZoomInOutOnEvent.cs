using Cinemachine;
using DG.Tweening;
using HalfBlind.ScriptableVariables;
using UnityEngine;

namespace GameCamera
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class ZoomInOutOnEvent : MonoBehaviour
    {
        [SerializeField] private ScriptableGameEvent _zoomInOnEvent;
        [SerializeField] private ScriptableGameEvent _zoomOutOnEvent;
        [SerializeField] private float _zoomPerEvent = 0.1f;
        [SerializeField] private float _zoomDuration = 0.2f;
        [SerializeField] private CinemachineVirtualCamera _camera;
        private Tween _tween;

        private void Reset()
        {
            _camera = GetComponent<CinemachineVirtualCamera>();
        }

        private void Start()
        {
            _zoomInOnEvent.AddListener(OnZoomInEvent);
            _zoomOutOnEvent.AddListener(OnZoomOutEvent);
        }

        private void OnDestroy()
        {
            _zoomInOnEvent.RemoveListener(OnZoomInEvent);
            _zoomOutOnEvent.RemoveListener(OnZoomOutEvent);
        }

        private void OnZoomInEvent()
        {
            _tween.Kill();
            _tween = DOTween.To(GetOrthographicSize,
                SetOrthographicSize,
                _camera.m_Lens.OrthographicSize - _zoomPerEvent, _zoomDuration).SetAutoKill(true);
        }

        private void OnZoomOutEvent()
        {
            _tween.Kill();
            _tween = DOTween.To(GetOrthographicSize,
                SetOrthographicSize,
                _camera.m_Lens.OrthographicSize + _zoomPerEvent, _zoomDuration).SetAutoKill(true);
        }

        private void SetOrthographicSize(float zoomLevel)
        {
            _camera.m_Lens.OrthographicSize = zoomLevel;
        }

        private float GetOrthographicSize()
        {
            return _camera.m_Lens.OrthographicSize;
        }
    }
}