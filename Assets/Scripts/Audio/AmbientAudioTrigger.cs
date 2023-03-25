using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Combat;
using RPG.Control;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Audio
{
    public class AmbientAudioTrigger : MonoBehaviour
    {
        [SerializeField] float trackFadeInTime = 1;
        [SerializeField] float trackFadeOutTime = 5;
        [SerializeField] List<CombatTarget> targets = new List<CombatTarget>();
        AmbientAudioPlayer audioPlayer;

        bool alreadyAggrevated = false;

        void Start()
        {
            audioPlayer = FindObjectOfType<AmbientAudioPlayer>();
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

            if(health.IsDead) return;

            targets.Add(target);
            controller.onAggrevated += FadeBattleMusic;
            health.onDie.AddListener(() => RemoveTarget(target));
        }

        void RemoveTarget(CombatTarget target)
        {
            var health = target.GetComponent<Health>();
            var controller = target.GetComponent<AIController>();

            targets.Remove(target);

            controller.onAggrevated -= FadeBattleMusic;
            health.onDie.RemoveListener(() => RemoveTarget(target));

            if(targets.Count == 0)
            {
                FadeAmbientMusic();
            }
        }

        void FadeBattleMusic()
        {   
            if(alreadyAggrevated) return;
            alreadyAggrevated = true;
            StartCoroutine(MusicRoutine(TrackType.Battle));
        }

        void FadeAmbientMusic()
        {
            if(!alreadyAggrevated) return;
            alreadyAggrevated = false;
            StartCoroutine(MusicRoutine(TrackType.Ambient));
        }

        IEnumerator MusicRoutine(TrackType trackType)
        {
            yield return audioPlayer.FadeOutCurrentTrack(trackFadeOutTime);
            yield return audioPlayer.FadeTrack(trackType, GetSceneIndex(), trackFadeInTime);
        }

        int GetSceneIndex()
        {
            return SceneManager.GetActiveScene().buildIndex;
        }
    }
}
