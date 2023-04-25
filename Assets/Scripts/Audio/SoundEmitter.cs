using RPG.Settings;
using UnityEngine;

namespace RPG.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public abstract class SoundEmitter : MonoBehaviour
    {
        [SerializeField] AudioSettingsSO audioSettings;
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
            audioSettings.onChange += UpdateVolume;
        }

        void OnDisable()
        {
            audioSettings.onChange -= UpdateVolume;
        }

        void UpdateVolume()
        {
            if(currentAudio == null) return;

            float baseVolume = audioSettings.GetMasterVolume() * currentAudio.GetVolumeFraction();

            switch(audioSetting)
            {
                case AudioSetting.Music:
                    audioSource.volume = audioSettings.GetMusicVolume() * baseVolume;
                    break;
                case AudioSetting.SFX:
                    audioSource.volume = audioSettings.GetSFXVolume() * baseVolume;
                    break;
            }
        }
    }
}
