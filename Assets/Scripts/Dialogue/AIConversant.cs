using System;
using RPG.Attributes;
using RPG.Control;
using UnityEngine;

namespace RPG.Dialogue
{
    public class AIConversant : MonoBehaviour, IRaycastable
    {
        [SerializeField] Dialogue defaultDialogue = null;
        [SerializeField] string conversantName = "";

        public string GetName()
        {
            return conversantName;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Dialogue;
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if(GetComponent<Health>().IsDead)
            {
                return false;
            }

            if(defaultDialogue == null)
            {
                return false;
            }

            if(Input.GetMouseButtonDown(0))
            {
                callingController.GetComponent<PlayerConversant>().StartDialogue(this, defaultDialogue);
            }

            return true;
        }
    }
}