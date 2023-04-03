using System.Collections;
using RPG.Core;
using UnityEngine;

namespace RPG.Audio
{
    public class SFXPlayer : MonoBehaviour
    {
        [SerializeField] GlobalSettings globalSettings;
        [SerializeField] AudioClip[] clips;
        [SerializeField] bool playFirstClipOnAwake = false;

        AudioSource audioSource;

        public void Play(int index)
        {
            audioSource.clip = clips[index];
            audioSource.Play();
        }

        public void PlayRandom()
        {
            int randomIndex = Random.Range(0, clips.Length);

            audioSource.clip = clips[randomIndex];
            audioSource.Play();
        }

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            UpdateVolume();

            if(playFirstClipOnAwake)
            {
                Play(0);
            }
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
            audioSource.volume = globalSettings.GetMasterVolume() * globalSettings.GetSFXVolume();
        }
    }
}
