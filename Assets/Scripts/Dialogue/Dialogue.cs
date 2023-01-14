using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{
    [CreateAssetMenu(menuName = ("New Dialogue"))]
    public class Dialogue : ScriptableObject
    {
        [SerializeField] List<DialogueNode> nodes;

        Dictionary<string, DialogueNode> nodeLookup = new Dictionary<string, DialogueNode>();

        void Awake()
        {
#if UNITY_EDITOR
            if(nodes.Count == 0)
            {
                nodes.Add(new DialogueNode());
            }
# endif
            // For Game Build Call
            OnValidate();
        }

        private void OnValidate()
        {
            nodeLookup.Clear();

            foreach(DialogueNode node in nodes)
            {
                nodeLookup[node.uniqueID] = node;
            }
        }

        public IEnumerable<DialogueNode> GetAllNodes()
        {
            return nodes;
        }

        public DialogueNode GetRootNode()
        {
            return nodes[0];
        }

        public IEnumerable<DialogueNode> GetAllChildren(DialogueNode parentNode)
        {
            foreach(string childID in parentNode.children)
            {
                if(!nodeLookup.ContainsKey(childID)) continue;
                
                yield return nodeLookup[childID];
            }
        }
    }
}
