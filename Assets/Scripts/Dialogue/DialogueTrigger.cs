using UnityEngine;
using UnityEngine.Events;

namespace RPG.Dialogue
{
    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField] string action;
        [SerializeField] UnityEvent onTrigger;

        public void Trigger(string actionTrigger)
        {
            if(action == actionTrigger)
            {
                onTrigger?.Invoke();
            }
        }
    }
}