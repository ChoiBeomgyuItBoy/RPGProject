using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{
    [System.Serializable]
    public class DialogueNode 
    {
        public string uniqueID;
        public string text;
        public Rect rect = new Rect(0, 0, 200, 100);
        public string[] children;
    }
}
