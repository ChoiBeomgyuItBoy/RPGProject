using UnityEngine;

namespace RPG.Dialogue
{
    [CreateAssetMenu(menuName = ("New Dialogue"))]
    public class Dialogue : ScriptableObject
    {
        [SerializeField] DialogueNode[] nodes;
    }
}
