using RPG.Core;
using UnityEngine;

namespace RPG.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundEmitter : MonoBehaviour
    {
        [SerializeField] GlobalSettings globalSettings;
        [SerializeField] AudioSetting audioSetting;
        AudioConfig currentAudio = null;
        AudioSource audioSource;

        enum AudioSetting
        {
            Music,
            SFX
        }

        public void PlayAudio(AudioConfig audioConfig)
        {
            if(audioConfig == null)
            {
                audioSource.clip = null;
                return;
            }

            if(currentAudio != null && currentAudio.HasToResume())
            {
                currentAudio.SetResumeTime(audioSource.time);
            }

            currentAudio = audioConfig;
            audioSource.Stop();
            audioSource.time = currentAudio.GetResumeTime();
            audioSource.clip = currentAudio.GetClip();
            audioSource.loop = currentAudio.HasToLoop();
            audioSource.Play();
            UpdateVolume();
        }

        void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        void OnEnable()
        {
            globalSettings.onSettingsChanged += UpdateVolume;
        }

        void OnDisable()
        {
            globalSettings.onSettingsChanged -= UpdateVolume;
        }

        void UpdateVolume()
        {
            if(currentAudio == null) return;

            float baseVolume = globalSettings.GetMasterVolume() * currentAudio.GetVolumeFraction();

            switch(audioSetting)
            {
                case AudioSetting.Music:
                    audioSource.volume = globalSettings.GetMusicVolume() * baseVolume;
                    break;
                case AudioSetting.SFX:
                    audioSource.volume = globalSettings.GetSFXVolume() * baseVolume;
                    break;
            }
        }
    }
}
