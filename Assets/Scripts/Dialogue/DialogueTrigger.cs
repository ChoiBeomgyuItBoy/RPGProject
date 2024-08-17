using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Dialogue
{
    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField] TriggerSetup[] triggersSetup;
        Dictionary<string, UnityEvent> actionLookup;

        [System.Serializable]
        struct TriggerSetup
        {
            public string actionID;
            public UnityEvent trigger;
        }

        public void Trigger(string caller, string actionID)
        {
            if(actionLookup == null)
            {
                FillLookup();
            }

            if(!actionLookup.ContainsKey(actionID))
            {
                Debug.LogError($"Action ID '{actionID}' not found in dialogue '{caller}'");
                return;
            }

            actionLookup[actionID].Invoke();
        }

        private void FillLookup()
        {
            actionLookup = new();

            foreach(var setup in triggersSetup)
            {
                actionLookup[setup.actionID] = setup.trigger;
            }
        }
    }
}