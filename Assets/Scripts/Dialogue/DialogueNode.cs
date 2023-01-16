using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Dialogue
{
    public class DialogueNode : ScriptableObject
    {
        [SerializeField] bool isPlayerSpeaking = false;
        [SerializeField] string text;
        [SerializeField] Rect rect = new Rect(0, 0, 200, 100);
        [SerializeField] List <string> children = new List<string>();

        public Rect GetRect() => rect;
        public string GetText() => text;
        public List<string> GetChildren() => children;
        public bool IsPlayerSpeaking() => isPlayerSpeaking;

# if UNITY_EDITOR
        public void SetPosition(Vector2 newPosition)
        {
            Undo.RecordObject(this, "Moved Dialogue Node");
            rect.position = newPosition;
            EditorUtility.SetDirty(this);
        }

        public void SetText(string newText)
        {
            if(newText == text) return;

            Undo.RecordObject(this, "Updated Dialogue Text");
            text = newText;
            EditorUtility.SetDirty(this);
        }

        public void AddChild(string childID)
        {
            Undo.RecordObject(this, "Linked Dialogue");
            children.Add(childID);
            EditorUtility.SetDirty(this);
        }

        public void RemoveChild(string childID)
        {
            Undo.RecordObject(this, "Unlinked Dialogue");
            children.Remove(childID);
            EditorUtility.SetDirty(this);
        }

        public void SetPlayerSpeaking(bool state)
        {
            Undo.RecordObject(this, "Changed Dialogue Speaker");
            isPlayerSpeaking = state;
            EditorUtility.SetDirty(this);
        }
    }
# endif
}
