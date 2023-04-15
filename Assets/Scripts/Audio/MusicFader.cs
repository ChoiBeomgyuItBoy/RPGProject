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

        protected IEnumerator FadeOutInMusic(Track track, bool lowVolume)
        { 
            yield return FadeOutMusic();
            yield return lowVolume? FadeInMusicLowVolume(track) : FadeInMusic(track);
        }

        protected IEnumerator FadeInMusic(Track track)
        {
            audioManager.value.PlayTrack(track);
            yield return audioManager.value.FadeInMusic(fadeInMusicTime);
        }

        protected IEnumerator FadeInMusicLowVolume(Track track)
        {
            audioManager.value.PlayTrack(track);
            yield return audioManager.value.FadeMusicLowVolume(fadeInMusicTime);
        }

        protected IEnumerator FadeOutMusic()
        {
            yield return audioManager.value.FadeOutMusic(fadeOutMusicTime);
        }
    }
}
