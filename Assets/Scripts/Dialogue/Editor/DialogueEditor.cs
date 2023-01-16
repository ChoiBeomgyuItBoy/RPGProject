using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System;

namespace RPG.Dialogue.Editor
{
    public class DialogueEditor : EditorWindow
    {
        Dialogue selectedDialogue = null;
        Vector2 scrollPosition;
        [NonSerialized] GUIStyle nodeStyle = null;
        [NonSerialized] GUIStyle playerNodeStyle = null;
        [NonSerialized] DialogueNode draggingNode = null;
        [NonSerialized] Vector2 draggingOffset;
        [NonSerialized] DialogueNode creatingNode = null;
        [NonSerialized] DialogueNode deletingNode = null;
        [NonSerialized] DialogueNode linkingParentNode = null;
        [NonSerialized] bool draggingCanvas = false;
        [NonSerialized] Vector2 draggingCanvasOffset;

        const float canvasSize = 4000;
        const float backgroundSize = 50;


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
            SetUpPlayerNodeStyle();
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

        private void SetUpPlayerNodeStyle()
        {
            playerNodeStyle = new GUIStyle();
            playerNodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;
            playerNodeStyle.padding = new RectOffset(20,20,20,20);
            playerNodeStyle.border = new RectOffset(12,12,12,12);
        }

        private void OnGUI()
        {
            if(selectedDialogue == null)
            {
                EditorGUILayout.LabelField("No Dialogue Selected.");
                return;
            }

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            Rect canvas = GUILayoutUtility.GetRect(canvasSize, canvasSize);
            Texture2D backgroundTexture = Resources.Load("background") as Texture2D;
            Rect textureCoordinates = new Rect(0, 0, canvasSize / backgroundSize, canvasSize / backgroundSize);

            GUI.DrawTextureWithTexCoords(canvas, backgroundTexture, textureCoordinates);

            foreach(DialogueNode node in selectedDialogue.GetAllNodes())
            {
                DrawConnections(node);
            }

            foreach(DialogueNode node in selectedDialogue.GetAllNodes())
            {
                DrawNode(node);
            }

            EditorGUILayout.EndScrollView();

            if(creatingNode != null)
            {
                selectedDialogue.CreateNode(creatingNode);
                creatingNode = null;
            }

            if(deletingNode != null)
            {
                selectedDialogue.DeleteNode(deletingNode);
                deletingNode = null;
            }

            ProcessEvents();
        }

        private void DrawNode(DialogueNode node)
        {
            GUIStyle style = nodeStyle;

            if(node.IsPlayerSpeaking())
            {
                style = playerNodeStyle;
            }

            GUILayout.BeginArea(node.GetRect(), style);

            node.SetText(EditorGUILayout.TextField(node.GetText()));
            
            GUILayout.BeginHorizontal();
            
            if (GUILayout.Button("x"))
            {
                deletingNode = node;
            }

            DrawLinkButtons(node);

            if (GUILayout.Button("+"))
            {
                creatingNode = node;
            }

            GUILayout.EndHorizontal();

            GUILayout.EndArea();
        }

        private void DrawLinkButtons(DialogueNode node)
        {
            if (linkingParentNode == null)
            {
                if (GUILayout.Button("link"))
                {
                    linkingParentNode = node;
                }
            }
            else if (linkingParentNode == node)
            {
                if (GUILayout.Button("cancel"))
                {
                    linkingParentNode = null;
                }
            }
            else if(linkingParentNode.GetChildren().Contains(node.name))
            {
                if(GUILayout.Button("unlink"))
                {
                    linkingParentNode.RemoveChild(node.name);
                    linkingParentNode = null;
                }
            }
            else
            {
                if (GUILayout.Button("child"))
                {
                    linkingParentNode.AddChild(node.name);
                    linkingParentNode = null;
                }
            }
        }

        private void DrawConnections(DialogueNode node)
        {
            Vector3 startPosition = new Vector2(node.GetRect().xMax, node.GetRect().center.y);

            foreach(DialogueNode childNode in selectedDialogue.GetAllChildren(node))
            {
                Vector3 endPosition = new Vector2(childNode.GetRect().xMin, childNode.GetRect().center.y);
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
    
                    draggingNode = GetNodeAtPoint(Event.current.mousePosition + scrollPosition);

                    if(draggingNode != null)
                    {
                        draggingOffset = draggingNode.GetRect().position - Event.current.mousePosition;
                        Selection.activeObject = draggingNode;
                    }
                    
                    if(draggingNode == null)
                    {
                        draggingCanvas = true;
                        draggingCanvasOffset = Event.current.mousePosition + scrollPosition;
                        Selection.activeObject = selectedDialogue;
                    }

                    break;
                
                case EventType.MouseDrag when draggingNode != null:

                    draggingNode.SetPosition(Event.current.mousePosition + draggingOffset);
                    GUI.changed = true;

                    break;

                case EventType.MouseDrag when draggingCanvas:

                    scrollPosition = draggingCanvasOffset - Event.current.mousePosition;
                    GUI.changed = true;
                    break;

                case EventType.MouseUp when draggingNode != null:

                    draggingNode = null;  
                    break;
                
                case EventType.MouseUp when draggingCanvas:

                    draggingCanvas = false;
                    break;
            }
        }

        private DialogueNode GetNodeAtPoint(Vector2 point)
        {
            DialogueNode foundNode = null;

            foreach(DialogueNode dialogueNode in selectedDialogue.GetAllNodes())
            {
                if(!dialogueNode.GetRect().Contains(point)) continue;
                
                foundNode = dialogueNode;
            }

            return foundNode;
        }
    }
}
