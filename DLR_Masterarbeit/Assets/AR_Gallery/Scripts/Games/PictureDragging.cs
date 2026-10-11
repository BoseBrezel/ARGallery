using UnityEngine;
using UnityEngine.EventSystems;

public class CalibrationDrag : MonoBehaviour, IDragHandler
{
    [SerializeField] private RectTransform target;

    public void OnDrag(PointerEventData eventData)
    {
        target.anchoredPosition += eventData.delta;
    }
}