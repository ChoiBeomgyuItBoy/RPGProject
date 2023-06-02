using RPG.Control;
using UnityEngine;

namespace RPG.Stats
{
    public class TraitEnhancer : MonoBehaviour, IRaycastable
    {
        [SerializeField] bool raycastable = false;

        CursorType IRaycastable.GetCursorType()
        {
            return CursorType.Trait;
        }

        bool IRaycastable.HandleRaycast(PlayerController callingController)
        {
            if(!raycastable) return false;

            if(Input.GetKeyDown(callingController.GetInteractionKey()))
            {
                callingController.GetComponent<TraitStore>().SetTraitEnhancer(this);
            }

            return true;
        }
    }
}
