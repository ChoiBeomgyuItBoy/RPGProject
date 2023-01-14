using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System;

namespace RPG.Dialogue.Editor
{
    public class DialogueEditor : EditorWindow
    {
        Dialogue selectedDialogue = null;
        GUIStyle nodeStyle = null;
        DialogueNode draggingNode = null;
        Vector2 draggingOffset;

        [MenuItem("Window/Dialogue Editor")]
        public static void ShowEditorWindow()
        {
            GetWindow(typeof(DialogueEditor), false, "Dialogue Editor");
        }

        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            Dialogue dialogue = EditorUtility.InstanceIDToObject(instanceID) as Dialogue;

            if(dialogue != null)
            {
                ShowEditorWindow();
                return true;
            }

            return false;
        }

        private void OnEnable()
        {
            Selection.selectionChanged += OnSelectionChanged;

            SetUpNodeStyle();
        }

        private void OnSelectionChanged()
        {
            Dialogue newDialogue = Selection.activeObject as Dialogue;

            if(newDialogue != null)
            {
                selectedDialogue = newDialogue;
                Repaint();
            }
        }

        private void SetUpNodeStyle()
        {
            nodeStyle = new GUIStyle();
            nodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
            nodeStyle.padding = new RectOffset(20,20,20,20);
            nodeStyle.border = new RectOffset(12,12,12,12);
        }

        private void OnGUI()
        {
            if(selectedDialogue == null)
            {
                EditorGUILayout.LabelField("No Dialogue Selected.");
                return;
            }

            foreach(DialogueNode node in selectedDialogue.GetAllNodes())
            {
                DrawConnections(node);
            }

            foreach(DialogueNode node in selectedDialogue.GetAllNodes())
            {
                DrawNode(node);
            }

            ProcessEvents();
        }

        private void DrawNode(DialogueNode node)
        {
            GUILayout.BeginArea(node.rect, nodeStyle);

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.LabelField("Node:");

            string newText = EditorGUILayout.TextField(node.text);
            string newUniqueID = EditorGUILayout.TextField(node.uniqueID);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(selectedDialogue, "Update Dialogue Text");

                node.text = newText;
                node.uniqueID = newUniqueID;
            }

            GUILayout.EndArea();
        }

        private void DrawConnections(DialogueNode node)
        {
            Vector3 startPosition = new Vector2(node.rect.xMax, node.rect.center.y);

            foreach(DialogueNode childNode in selectedDialogue.GetAllChildren(node))
            {
                Vector3 endPosition = new Vector2(childNode.rect.xMin, childNode.rect.center.y);
                Vector3 controlPointOffset = endPosition - startPosition;
                controlPointOffset.y = 0;
                controlPointOffset.x *= 0.8f;
               
                Handles.DrawBezier(
                    startPosition, endPosition, 
                    startPosition + controlPointOffset, 
                    endPosition - controlPointOffset, 
                    Color.white, null, 4f
                );
            }
        }


        private void ProcessEvents()
        {
            switch(Event.current.type)
            {
                case EventType.MouseDown when draggingNode == null:
    
                    draggingNode = GetNodeAtPoint(Event.current.mousePosition);

                    if(draggingNode == null) break;
                    
                    draggingOffset = draggingNode.rect.position - Event.current.mousePosition;
                    
                    break;
                
                case EventType.MouseDrag when draggingNode != null:

                    Undo.RecordObject(selectedDialogue, "Move Dialogue Node");
                    draggingNode.rect.position = Event.current.mousePosition + draggingOffset;
                    GUI.changed = true;
                    break;

                case EventType.MouseUp when draggingNode != null:

                    draggingNode = null;  
                    break;
            }
        }

        private DialogueNode GetNodeAtPoint(Vector2 point)
        {
            DialogueNode foundNode = null;

            foreach(DialogueNode dialogueNode in selectedDialogue.GetAllNodes())
            {
                if(!dialogueNode.rect.Contains(point)) continue;
                
                foundNode = dialogueNode;
            }

            return foundNode;
        }
    }
}
