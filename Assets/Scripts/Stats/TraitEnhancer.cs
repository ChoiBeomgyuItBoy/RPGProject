using RPG.Control;
using UnityEngine;

namespace RPG.Stats
{
    public class TraitEnhancer : MonoBehaviour, IRaycastable
    {
        CursorType IRaycastable.GetCursorType()
        {
            return CursorType.Trait;
        }

        bool IRaycastable.HandleRaycast(PlayerController callingController)
        {
            if(Input.GetKeyDown(callingController.GetInteractionKey()))
            {
                callingController.GetComponent<TraitStore>().SetTraitEnhancer(this);
            }

            return true;
        }
    }
}
