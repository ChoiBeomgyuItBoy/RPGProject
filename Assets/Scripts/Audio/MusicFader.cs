using System;
using System.Collections;
using GameDevTV.Utils;
using UnityEngine;

namespace RPG.Audio
{
    public abstract class MusicFader : MonoBehaviour
    {
        [SerializeField] float fadeInMusicTime = 2;
        [SerializeField] float fadeOutMusicTime = 2;
        protected LazyValue<AudioManager> audioManager;

        private void Start()
        {
            audioManager = new LazyValue<AudioManager>(() => FindObjectOfType<AudioManager>());
            audioManager.ForceInit();
            OnStart()?.Invoke();
        }

        protected virtual Action OnStart()
        {
            return null;
        }

        protected IEnumerator FadeOutInMusic(Track track, bool inDialogue)
        { 
            yield return FadeOutMusic();
            yield return FadeInMusic(track, inDialogue);
        }

        protected IEnumerator FadeInMusic(Track track, bool inDialogue)
        {
            yield return audioManager.value.FadeInTrack(fadeInMusicTime, track, inDialogue);
        }

        protected IEnumerator FadeOutMusic()
        {
            yield return audioManager.value.FadeOutTrack(fadeOutMusicTime);
        }
    }
}
