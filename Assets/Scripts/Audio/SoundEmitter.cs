using RPG.Core;
using UnityEngine;

namespace RPG.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public abstract class SoundEmitter : MonoBehaviour
    {
        [SerializeField] PlayerSettings playerSettings;
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
            playerSettings.onSettingsChanged += UpdateVolume;
        }

        void OnDisable()
        {
            playerSettings.onSettingsChanged -= UpdateVolume;
        }

        void UpdateVolume()
        {
            if(currentAudio == null) return;

            float baseVolume = playerSettings.GetMasterVolume() * currentAudio.GetVolumeFraction();

            switch(audioSetting)
            {
                case AudioSetting.Music:
                    audioSource.volume = playerSettings.GetMusicVolume() * baseVolume;
                    break;
                case AudioSetting.SFX:
                    audioSource.volume = playerSettings.GetSFXVolume() * baseVolume;
                    break;
            }
        }
    }
}
