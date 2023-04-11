using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    [RequireComponent(typeof(PlayableDirector))]
    public class CinematicTrigger : MonoBehaviour
    {
        public void PlayCinematic()
        {
            GetComponent<PlayableDirector>().Play();
        }
    }
}


