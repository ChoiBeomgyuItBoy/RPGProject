using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;

namespace RPG.Cinematics
{
    public class CinematicEventHandler : MonoBehaviour
    {
        [SerializeField] UnityEvent onCinematicPlayed;
        [SerializeField] UnityEvent onCinematicStopped;

        GameObject player;

        void Awake()
        {
            player = GameObject.FindWithTag("Player");
        }

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
