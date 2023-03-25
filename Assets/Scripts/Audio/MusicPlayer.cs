using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Audio
{
    public class MusicPlayer : MonoBehaviour
    {
        [SerializeField] GlobalSettings globalSettings;
        [SerializeField] SceneTrackData[] sceneTracks;
        [SerializeField] float initialTrackFadeInTime = 10;
        Dictionary<int, Dictionary<TrackType, Track>> trackLookup = null;
        Track currentTrack = null;
        Coroutine currentActiveFade = null;
        AudioSource audioSource;  


        [System.Serializable]
        class SceneTrackData
        {
            public int sceneIndex = 0;
            public Track[] tracks;
        }

        [System.Serializable]
        class Track
        {
            public TrackType trackType = default;
            public AudioClip clip;
            [Range(0,1)] public float volumeFraction = 0.2f;
            [System.NonSerialized] public float resumeTime = 0;
        }

        public bool HasTrack(TrackType trackType, int sceneIndex)
        {
            if(trackLookup.ContainsKey(sceneIndex))
            {
                var innerLookup = trackLookup[sceneIndex];

                if(innerLookup.ContainsKey(trackType))
                {
                    return true;
                }
            }
            return false;
        }

        public Coroutine FadeTrack(TrackType trackType, int sceneIndex, float time)
        {
            if(!HasTrack(trackType, sceneIndex)) return null;

            var track = trackLookup[sceneIndex][trackType];

            PlayNewTrack(track);

            return Fade(track.volumeFraction, time);
        }

        public Coroutine FadeOutCurrentTrack(float time)
        {
            return Fade(0, time);
        }
        
        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();

            BuildTrackLookup();
        }

        private IEnumerator Start()
        {
            int currentScene = SceneManager.GetActiveScene().buildIndex;
            yield return FadeTrack(TrackType.Ambient, currentScene, initialTrackFadeInTime);
        }

        private void OnEnable()
        {
            globalSettings.onSettingsChanged += UpdateVolume;
        }

        private void OnDisable()
        {
            globalSettings.onSettingsChanged -= UpdateVolume;
        }

        private void UpdateVolume()
        {
            if(currentTrack != null)
            {
                audioSource.volume = globalSettings.GetMasterVolume() * globalSettings.GetMusicVolume() * Mathf.Clamp01(currentTrack.volumeFraction);
            }
        }

        private void BuildTrackLookup()
        {
            trackLookup = new Dictionary<int, Dictionary<TrackType, Track>>();

            foreach(var sceneTrack in sceneTracks)
            {
                var innerLookup = new Dictionary<TrackType, Track>();

                foreach(var track in sceneTrack.tracks)
                {
                    innerLookup[track.trackType] = track;
                }

                trackLookup[sceneTrack.sceneIndex] = innerLookup;
            }
        }

        private void PlayNewTrack(Track track)
        {
            if(currentTrack != null)
            {
                currentTrack.resumeTime = audioSource.time;
            }

            currentTrack = track;
            audioSource.clip = currentTrack.clip;
            audioSource.time = currentTrack.resumeTime;
            audioSource.Play();
        }

        private Coroutine Fade(float volumeFraction, float time)
        {
            if(currentActiveFade != null)
            {
                StopCoroutine(currentActiveFade);
            }

            currentActiveFade = StartCoroutine(FadeRoutine(volumeFraction, time));
            return currentActiveFade;
        }

        private IEnumerator FadeRoutine(float volumeFraction, float time)
        {
            float targetVolume = globalSettings.GetMasterVolume() * globalSettings.GetMusicVolume() * Mathf.Clamp01(volumeFraction);

            while(!Mathf.Approximately(audioSource.volume, targetVolume))
            {
                audioSource.volume = Mathf.MoveTowards(audioSource.volume, targetVolume, Time.unscaledDeltaTime / time);
                yield return null;
            }
        }
    }
}

