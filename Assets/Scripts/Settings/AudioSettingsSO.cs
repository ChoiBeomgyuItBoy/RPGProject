using System;
using UnityEngine;

namespace RPG.Settings
{
    [CreateAssetMenu(menuName = "RPG/Settings/New Audio Settings")]
    public class AudioSettingsSO : ScriptableObject
    {  
        [SerializeField] [Range(0,1)] float masterVolume = 1;
        [SerializeField] [Range(0,1)] float musicVolume = 1;
        [SerializeField] [Range(0,1)] float sfxVolume = 1;

        public event Action onChange;

        public float GetMasterVolume() => masterVolume;
        public float GetMusicVolume() => musicVolume;
        public float GetSFXVolume() => sfxVolume;

        public void SetMasterVolume(float masterVolume)
        {
            this.masterVolume = masterVolume;
            onChange?.Invoke();
        }

        public void SetMusicVolume(float musicVolume)
        {
            this.musicVolume = musicVolume;
            onChange?.Invoke();
        }

        public void SetSFXVolume(float sfxVolume)
        {
            this.sfxVolume = sfxVolume;
            onChange?.Invoke();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            onChange?.Invoke();
        }
#endif
    }
}
