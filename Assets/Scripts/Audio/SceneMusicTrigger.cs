using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Audio
{
    public class SceneMusicTrigger : MusicFader
    {
        [SerializeField] SceneTrack[] sceneTracks;
        Dictionary<int, SceneTrack> trackLookup = new Dictionary<int, SceneTrack>();

        [System.Serializable]
        class SceneTrack
        {
            public int sceneIndex = 0;
            public Track ambientTrack;
            public Track combatTrack;
        }

        protected override Action OnStart()
        {
            return PlayAmbientMusic;
        }

        public void PlayAmbientMusic()
        {
            if(!trackLookup.ContainsKey(GetSceneIndex())) return;

            Track ambientTrack = trackLookup[GetSceneIndex()].ambientTrack;

            if(ambientTrack != null)
            {
                StartCoroutine(FadeOutInMusic(ambientTrack));
            }
        }

        public void PlayCombatMusic()
        {
            if(!trackLookup.ContainsKey(GetSceneIndex())) return;

            Track combatTrack = trackLookup[GetSceneIndex()].combatTrack;

            if(combatTrack != null)
            {
                StartCoroutine(FadeOutInMusic(combatTrack));
            }
        }

        void Awake()
        {
            foreach(var sceneTrack in sceneTracks)
            {
                trackLookup[sceneTrack.sceneIndex] = sceneTrack;
            }
        }

        int GetSceneIndex()
        {
            return SceneManager.GetActiveScene().buildIndex;
        }
    }
}