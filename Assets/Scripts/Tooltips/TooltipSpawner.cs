using UnityEngine;
using UnityEngine.EventSystems;

namespace Tooltips
{
    public class TooltipSpawner : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public string Text;
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            var tooltipManager = TooltipManager.Find();
            tooltipManager.DisplayTooltip(transform, Text);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            var tooltipManager = TooltipManager.Find();
            if (tooltipManager != null)
            {
                tooltipManager.HideTooltip(transform);
            }
        }
    }
}