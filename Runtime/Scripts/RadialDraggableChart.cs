using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityCharts
{
    public class RadialDraggableChart : Chart, IPointerDownHandler, IDragHandler
    {
        [SerializeField] private bool smoothRotation;
        
        private Quaternion _dragStartRotation;
        private Quaternion _dragStartInverseRotation;

        public void OnPointerDown(PointerEventData eventData)
        {
            _dragStartRotation = transform.localRotation;
            if (DragWorldPoint(eventData, out var worldPoint))
            {
                _dragStartInverseRotation = Quaternion
                    .Inverse(Quaternion.LookRotation(worldPoint - transform.position, Vector3.forward));
            }
            else
            {
                Debug.LogWarning("Couldn't get drag start world point");
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!DragWorldPoint(eventData, out var worldPoint)) return;
            var currentDragAngle = Quaternion.LookRotation(worldPoint - transform.position, 
                Vector3.forward);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y,
                (currentDragAngle * _dragStartInverseRotation * _dragStartRotation).eulerAngles.z);
        }

        private bool DragWorldPoint(PointerEventData eventData, out Vector3 worldPoint)
        {
            return RectTransformUtility.ScreenPointToWorldPointInRectangle(GetComponent<RectTransform>(),
                eventData.position, eventData.pressEventCamera, out worldPoint);
        }
    }
}