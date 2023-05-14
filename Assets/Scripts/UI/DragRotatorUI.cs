using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.UI
{
    public class DragRotatorUI : MonoBehaviour, IDragHandler
    {
        [SerializeField] Transform model;

        public void OnDrag(PointerEventData eventData)
        {
            model.Rotate(Vector3.up, -eventData.delta.x, Space.World);
        }

    }
}
