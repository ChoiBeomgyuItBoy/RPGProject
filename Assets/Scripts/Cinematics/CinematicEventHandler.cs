using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;

namespace RPG.Cinematics
{
    [RequireComponent(typeof(PlayableDirector))]
    public class CinematicEventHandler : MonoBehaviour
    {
        [SerializeField] UnityEvent onCinematicPlayed;
        [SerializeField] UnityEvent onCinematicStopped;

        void OnEnable()
        {
            GetComponent<PlayableDirector>().played += OnPlayed;
            GetComponent<PlayableDirector>().stopped += OnStopped;
        }
        
        void OnDisable()
        {
            GetComponent<PlayableDirector>().played -= OnPlayed;
            GetComponent<PlayableDirector>().stopped -= OnStopped;
        }

        void OnPlayed(PlayableDirector director)
        {
            onCinematicPlayed?.Invoke();
        }

        void OnStopped(PlayableDirector director)
        {
            onCinematicStopped?.Invoke();
        }
    }
}
