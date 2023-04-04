using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Combat;
using RPG.Control;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Audio
{
    public class MusicTrigger : MonoBehaviour
    {
        [SerializeField] float fadeInMusicTime = 2;
        [SerializeField] float fadeOutMusicTime = 2;
        AudioManager audioManager;
        List<CombatTarget> targets = new List<CombatTarget>();

        int sceneIndex => SceneManager.GetActiveScene().buildIndex;
        bool alreadyInCombat = false;

        void Start()
        {
            audioManager = FindObjectOfType<AudioManager>();
            StartCoroutine(FadeInMusic(TrackType.Ambient));
        }

        void OnTriggerEnter(Collider other)
        {
            var target = other.GetComponent<CombatTarget>();

            if(target != null)
            {
                AddTarget(target);
            }
        }

        void OnTriggerExit(Collider other)
        {
            var target = other.GetComponent<CombatTarget>();

            if(target != null)
            {
                RemoveTarget(target);
            }
        }

        void AddTarget(CombatTarget target)
        {
            var health = target.GetComponent<Health>();
            var controller = target.GetComponent<AIController>();

            if(health != null && controller != null && !health.IsDead)
            {
                targets.Add(target);
                health.onDie.AddListener(() => RemoveTarget(target));
                controller.onAggrevated += PlayCombatMusic;
            }
        }

        void RemoveTarget(CombatTarget target)
        {
            var health = target.GetComponent<Health>();
            var controller = target.GetComponent<AIController>();

            if(health != null && controller != null)
            {
                targets.Remove(target);
                health.onDie.RemoveListener(() => RemoveTarget(target));
                controller.onAggrevated -= PlayCombatMusic;

                if(targets.Count == 0 && alreadyInCombat)
                {
                    PlayAmbientMusic();
                } 
            }
        }

        void PlayCombatMusic()
        {
            if(alreadyInCombat) return;
            alreadyInCombat = true;
            StartCoroutine(FadeOutInMusic(TrackType.Combat));
        }

        void PlayAmbientMusic()
        {
            alreadyInCombat = false;
            StartCoroutine(FadeOutInMusic(TrackType.Ambient));
        }

        IEnumerator FadeOutInMusic(TrackType trackType)
        {
            yield return FadeOutMusic();
            yield return FadeInMusic(trackType);
        }

        IEnumerator FadeOutMusic()
        {
            yield return audioManager.FadeOutMusic(fadeOutMusicTime);
        }

        IEnumerator FadeInMusic(TrackType trackType)
        {
            yield return audioManager.FadeInMusic(fadeInMusicTime, sceneIndex, trackType);
        }
    }
}
