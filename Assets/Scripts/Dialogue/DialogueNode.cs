using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{
    [System.Serializable]
    public class DialogueNode 
    {
        [SerializeField] string uniqueID;
        [SerializeField] string dialogueText;
        [SerializeField] string[] children;
    }
}
