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

        public void PlayAudio(AudioConfig audioConfig)
        {
            if(currentAudio != null && currentAudio.HasToResume())
            {
                currentAudio.SetResumeTime(audioSource.time);
            }

            if(audioConfig == null)
            {
                audioSource.clip = null;
                return;
            }

            currentAudio = audioConfig;
            audioSource.Stop();
            audioSource.clip = currentAudio.GetClip();
            audioSource.time = currentAudio.GetResumeTime();
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
            audioSource.volume = audioSettings.GetVolume(audioSetting) * 
            audioSettings.GetVolume(AudioSetting.Master) * 
            currentAudio.GetVolumeFraction();
        }
    }
}
