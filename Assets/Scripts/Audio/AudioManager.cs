using System.Collections;
using RPG.Core;
using UnityEngine;
using UnityEngine.Audio;

namespace RPG.Audio
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] AudioMixer audioMixer;
        [SerializeField] GlobalSettings globalSettings;
        [SerializeField] float initalFadeInTime = 4;
        Track currentTrack = null;
        AudioSource audioSource;  

        public Coroutine FadeOutMaster(float time)
        {
            return StartCoroutine(FadeSnapshot("FadeOutMaster", time));
        }

        public Coroutine FadeInMaster(float time)
        {
            return StartCoroutine(FadeSnapshot("FadeInMaster", time));
        }

        public Coroutine FadeOutTrack(float time)
        {
            return StartCoroutine(FadeSnapshot("FadeOutMusic", time));
        }

        public Coroutine FadeInTrack(float time, Track track, bool inDialogue)
        {
            PlayTrack(track);

            if(inDialogue)
            {
                return StartCoroutine(FadeSnapshot("FadeInLowerMusic", time));
            }
            else
            {
                return StartCoroutine(FadeSnapshot("FadeInMusic", time));
            }
        }

        public Coroutine FadeInLowerMusic(float time)
        {
            return StartCoroutine(FadeSnapshot("FadeInLowerMusic", time));
        }

        public void PlayTrack(Track track)
        {
            if(currentTrack != null)
            {
                currentTrack.SetResumeTime(audioSource.time);
            }

            if(track == null)
            {
                audioSource.clip = null;
                return;
            }

            currentTrack = track;
            audioSource.clip = currentTrack.GetClip();
            audioSource.time = currentTrack.GetResumeTime();
            UpdateVolume();
            audioSource.Play();
        }

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private IEnumerator Start()
        {
            yield return FadeInMaster(initalFadeInTime);
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
                audioSource.volume = globalSettings.GetMasterVolume() * 
                globalSettings.GetMusicVolume() * 
                Mathf.Clamp01(currentTrack.GetVolumeFraction());
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
