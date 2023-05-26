using GameDevTV.Utils;
using UnityEngine;

namespace RPG.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public abstract class SoundEmitter : MonoBehaviour
    {
        [SerializeField] AudioSetting audioSetting;
        AudioConfig currentAudio = null;
        AudioSource audioSource;
        LazyValue<AudioManager> audioManager;

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

        public abstract void OnStartAction();

        void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        void Start()
        {
            audioManager = new LazyValue<AudioManager>( () => FindObjectOfType<AudioManager>() );
            audioManager.ForceInit();
            audioManager.value.onChange += UpdateVolume;
            OnStartAction();
        }

        void UpdateVolume()
        {
            if(currentAudio == null) return;
            if(audioSource == null) return;
            audioSource.volume = audioManager.value.GetVolume(audioSetting) * 
            audioManager.value.GetVolume(AudioSetting.Master) * 
            currentAudio.GetVolumeFraction();
        }
    }
}
