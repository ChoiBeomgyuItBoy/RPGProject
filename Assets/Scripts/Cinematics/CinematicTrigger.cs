using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour, ISaveable
    {
        [System.Serializable]
        class SerializableBool
        {
            bool state;

            public SerializableBool(bool state) { this.state = state; } 

            public bool ToBool() => state;
        }

        bool alreadyCollidedWith = false;

        void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Player" && !alreadyCollidedWith)
            {
                GetComponent<PlayableDirector>().Play();
                alreadyCollidedWith = true;
            }
        }

        public object CaptureState()
        {
            return new SerializableBool(alreadyCollidedWith);
        }

        public void RestoreState(object state)
        {
            SerializableBool previousState = (SerializableBool) state;

            alreadyCollidedWith = previousState.ToBool();
        }
    }
}


