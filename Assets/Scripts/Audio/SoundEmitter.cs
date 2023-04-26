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
            audioSource.volume = audioSettings.GetVolume(audioSetting);
        }
    }
}
