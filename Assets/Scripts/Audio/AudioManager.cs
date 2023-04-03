using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;
using UnityEngine.Audio;

namespace RPG.Audio
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] AudioMixer audioMixer;
        [SerializeField] GlobalSettings globalSettings;
        [SerializeField] SceneTrackData[] sceneTracks;
        Dictionary<int, Dictionary<TrackType, Track>> trackLookup = null;
        Track currentTrack = null;
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

        public Coroutine FadeOutMaster(float time)
        {
            return StartCoroutine(FadeSnapshot("FadeOutMaster", time));
        }

        public Coroutine FadeInMaster(float time)
        {
            return StartCoroutine(FadeSnapshot("FadeInMaster", time));
        }

        public Coroutine FadeOutMusic(float time)
        {
            return StartCoroutine(FadeSnapshot("FadeOutMusic", time));
        }

        public Coroutine FadeInMusic(float time, int sceneIndex, TrackType trackType)
        {
            PlayTrack(sceneIndex, trackType);
            return StartCoroutine(FadeSnapshot("FadeInMusic", time));
        }

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            BuildTrackLookup();
        }

        private IEnumerator Start()
        {
            UpdateVolume();
            yield return FadeInMaster(3);
        }

        private void OnEnable()
        {
            globalSettings.onSettingsChanged += UpdateVolume;
        }

        private void OnDisable()
        {
            globalSettings.onSettingsChanged -= UpdateVolume;
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

        private bool HasTrack(int sceneIndex, TrackType trackType)
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

        private void PlayTrack(int sceneIndex, TrackType trackType)
        {
            if(!HasTrack(sceneIndex, trackType)) 
            {
                Debug.LogWarning("Not a track of type " + trackType + " associated with this scene");
                audioSource.clip = null;
                return;
            }

            if(currentTrack != null)
            {
                currentTrack.resumeTime = audioSource.time;
            }

            currentTrack = trackLookup[sceneIndex][trackType];;
            audioSource.clip = currentTrack.clip;
            audioSource.time = currentTrack.resumeTime;
            audioSource.Play();
        }

        private void UpdateVolume()
        {
            if(currentTrack != null)
            {
                audioSource.volume = globalSettings.GetMasterVolume() * globalSettings.GetMusicVolume() * Mathf.Clamp01(currentTrack.volumeFraction);
            }
        } 

        private IEnumerator FadeSnapshot(string snapshotName, float time)
        {
            var snapshot = audioMixer.FindSnapshot(snapshotName);

            if(snapshot == null)
            {
                Debug.LogError($"Snapshot '{snapshotName}' not found");
                yield break;
            }

            snapshot.TransitionTo(time);
            yield return new WaitForSeconds(time);
        }
    }
}
