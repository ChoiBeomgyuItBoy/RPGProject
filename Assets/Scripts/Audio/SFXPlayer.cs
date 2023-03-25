using RPG.Core;
using UnityEngine;

namespace RPG.Audio
{
    public class SFXPlayer : MonoBehaviour
    {
        [SerializeField] GlobalSettings globalSettings;
        [SerializeField] AudioClip[] clips;

        AudioSource audioSource;

        public void Play(int index)
        {
            audioSource.clip = clips[index];
            audioSource.Play();
        }

        public void PlayRandom()
        {
            audioSource.clip = clips[GetRandomIndex()];
            audioSource.Play();
        }

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            UpdateVolume();
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

        private int GetRandomIndex()
        {
            return Random.Range(0, clips.Length);
        }
    }
}
