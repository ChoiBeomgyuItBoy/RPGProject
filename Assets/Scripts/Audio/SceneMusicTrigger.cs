using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Audio
{
    public class SceneMusicTrigger : MusicFader
    {
        [SerializeField] SceneTrack[] sceneTracks;
        Dictionary<int, Dictionary<TrackType, Track>> trackLookup = null;

        [System.Serializable]
        class SceneTrack
        {
            public int sceneIndex = 0;
            public SceneTrackData[] sceneTracks;
        }

        [System.Serializable]
        class SceneTrackData
        {
            public TrackType trackType = default;
            public Track track;
        }

        enum TrackType
        {
            Ambient, 
            Combat
        }

        public void FadeOutInAmbientMusic()
        {   
            FadeSceneTrack(TrackType.Ambient);
        }

        public void FadeOutInCombatMusic()
        {
            FadeSceneTrack(TrackType.Combat);
        }

        protected override Action OnStart()
        {
            return () => audioManager.value.PlayTrack(GetTrack(TrackType.Ambient));
        }

        private void FadeSceneTrack(TrackType trackType)
        {
            Track track = GetTrack(trackType);

            if(track != null)
            {
                StartCoroutine(FadeOutInMusic(track, false));
            }
        }

        private void Awake()
        {
            BuildTrackLookup();
        }

        private void BuildTrackLookup()
        {
            trackLookup = new Dictionary<int, Dictionary<TrackType, Track>>();

            foreach(var sceneTrack in sceneTracks)
            {
                var innerLookup = new Dictionary<TrackType, Track>();

                foreach(var track in sceneTrack.sceneTracks)
                {
                    innerLookup[track.trackType] = track.track;
                }

                trackLookup[sceneTrack.sceneIndex] = innerLookup;
            }
        }

        private Track GetTrack(TrackType trackType)
        {
            int sceneIndex = SceneManager.GetActiveScene().buildIndex;

            if(!trackLookup.ContainsKey(sceneIndex))
            {
                Debug.LogWarning($"Not a track of type {trackType} associated with scene {sceneIndex}");
                return null;
            }   

            return trackLookup[sceneIndex][trackType];
        }
    }
}