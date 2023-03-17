using GameDevTV.Inventories;
using UnityEngine;

namespace RPG.Control
{
    [RequireComponent(typeof(Pickup))]
    public class ClickablePickup : MonoBehaviour, IRaycastable
    {
        Pickup pickup = null;

        void Awake()
        {
            pickup = GetComponent<Pickup>();
        }

        CursorType IRaycastable.GetCursorType()
        {
            if(pickup.CanBePickedUp())
            {
                return CursorType.Pickup;
            }
            else
            {
                return CursorType.FullPickup;
            }
        }

        bool IRaycastable.HandleRaycast(PlayerController callingController)
        {
            if(Input.GetMouseButton(0))
            {
                pickup.PickupItem();
            }

            return true;
        }
    }
}
