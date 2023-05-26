using System.Collections;
using System.Collections.Generic;
using GameDevTV.Utils;
using UnityEngine;

namespace RPG.Audio
{
    public class MusicPlayer : SoundEmitter
    {
        [SerializeField] float fadeOutMusicTime = 1;
        [SerializeField] float fadeInMusicTime = 0.5f;
        [SerializeField] SceneMusicData thisSceneMusic;
        [SerializeField] AudioConfig[] additionalMusic;

        [System.Serializable]
        class SceneMusicData
        {
            public ConditionalMusic[] ambientMusic;
            public ConditionalMusic[] combatMusic;
        }

        [System.Serializable]
        class ConditionalMusic
        {
            public AudioConfig audioConfig;
            public Condition playCondition;
        }

        public override void OnStartAction()
        {
            PlayAudio(GetValidMusic(thisSceneMusic.ambientMusic));
        }

        public void FadeInAmbientMusic()
        {
            var validMusic = GetValidMusic(thisSceneMusic.ambientMusic);
            StartCoroutine(FadeOutInMusic(validMusic));
        }

        public void FadeInCombatMusic()
        {
            var validMusic = GetValidMusic(thisSceneMusic.combatMusic);
            StartCoroutine(FadeOutInMusic(validMusic));
        }

        public void FadeInAdditionalMusic(int index)
        {
            StartCoroutine(FadeOutInMusic(additionalMusic[index]));
        }

        private AudioConfig GetValidMusic(ConditionalMusic[] conditionalMusic)
        {   
            foreach(var track in conditionalMusic)
            {
                if(!track.playCondition.Check(GetPredicates())) continue;
                return track.audioConfig;
            }

            return null;
        }

        private IEnumerable<IPredicateEvaluator> GetPredicates()
        {
            return GameObject.FindWithTag("Player").GetComponents<IPredicateEvaluator>();
        }

        private IEnumerator FadeOutInMusic(AudioConfig audioConfig)
        {
            yield return audioManager.value.FadeOutMusic(fadeOutMusicTime);
            PlayAudio(audioConfig);
            yield return audioManager.value.FadeInMusic(fadeInMusicTime);
        }
    }
}
