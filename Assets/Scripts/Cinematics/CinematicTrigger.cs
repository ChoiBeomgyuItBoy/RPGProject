using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        private List<Collider> alreadyCollidedWith = new List<Collider>();

        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Player" && !alreadyCollidedWith.Contains(other))
            {
                GetComponent<PlayableDirector>().Play();
                alreadyCollidedWith.Add(other);
            }
        }
    }
}


