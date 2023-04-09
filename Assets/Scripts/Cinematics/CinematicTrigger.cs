using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace RPG.Cinematics
{
    [RequireComponent(typeof(PlayableDirector))]
    public class CinematicTrigger : MonoBehaviour
    {
        public void PlayCinematic(TimelineAsset timelineAsset)
        {
            GetComponent<PlayableDirector>().playableAsset = timelineAsset;
            GetComponent<PlayableDirector>().Play();
        }
    }
}


