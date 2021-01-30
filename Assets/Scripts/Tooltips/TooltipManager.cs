using TMPro;
using UnityEngine;

namespace Tooltips
{
    public class TooltipManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Transform _tooltipTransform;
        [SerializeField] private Transform _middleScreenTransform;
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private float _lineDistance = 4f;
        private Transform _currentTarget;

        private static TooltipManager _tooltipManager;
        
        public static TooltipManager Find()
        {
            if (_tooltipManager == null)
            {
                _tooltipManager = FindObjectOfType<TooltipManager>();
            }

            return _tooltipManager;
        }

        public void DisplayTooltip(Transform target, string text)
        {
            var targetPosition = target.position;

            var middleScreenPosition = _middleScreenTransform.position;
            var director = Vector3.MoveTowards(targetPosition, middleScreenPosition, _lineDistance);

            var tooltipPosition = director;
            _lineRenderer.SetPositions(new []{targetPosition, tooltipPosition});

            _tooltipTransform.position = tooltipPosition;
            _currentTarget = target;
            _text.text = text;
            _tooltipTransform.gameObject.SetActive(true);
        }

        public void HideTooltip(Transform target)
        {
            if (target == _currentTarget)
            {
                _tooltipTransform.gameObject.SetActive(false);
                _currentTarget = null;
            }
        }
    }
}