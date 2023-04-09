using System;
using GameDevTV.Saving;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Core
{
    public class TriggerHandler : MonoBehaviour, ISaveable
    {
        [SerializeField] UnityEvent onTriggerEnter;
        [SerializeField] UnityEvent onTriggerExit;
        [SerializeField] string tagToRecognize = "";
        [SerializeField] bool saveable = true;

        Boolean alreadyTriggered = false;

        void OnTriggerEnter(Collider other)
        {
            if(other.tag == tagToRecognize && !alreadyTriggered)
            {
                alreadyTriggered = true;
                onTriggerEnter?.Invoke();
            }
        }

        void OnTriggerExit(Collider other)
        {
            if(other.tag == tagToRecognize && !alreadyTriggered)
            {
                onTriggerExit?.Invoke();
            }
        }

        object ISaveable.CaptureState()
        {
            if(!saveable) return null;
            return alreadyTriggered;
        }

        void ISaveable.RestoreState(object state)
        {
            if(!saveable) return;
            alreadyTriggered = (bool) state;
        }
    }
}
