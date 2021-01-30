using TMPro;
using UnityEngine;

namespace Tooltips
{
    public class TooltipManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Transform _tooltipTransform;
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
            _tooltipTransform.position = new Vector3(targetPosition.x, targetPosition.y, _tooltipTransform.position.z);
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