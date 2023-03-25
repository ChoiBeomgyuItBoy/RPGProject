using System;
using UnityEngine;

namespace RPG.Core
{
    [CreateAssetMenu(menuName = "RPG/New Global Settings")]
    public class GlobalSettings : ScriptableObject
    {  
        [SerializeField] [Range(0,1)] float masterVolume = 1;
        [SerializeField] [Range(0,1)] float musicVolume = 1;
        [SerializeField] [Range(0,1)] float sfxVolume = 1;

        public event Action onSettingsChanged;

        public float GetMasterVolume()
        {
            return masterVolume;
        } 

        public float GetMusicVolume()
        {
            return musicVolume;
        }    

        public float GetSFXVolume()
        {
            return sfxVolume;
        }

        public void SetMasterVolume(float masterVolume)
        {
            this.masterVolume = masterVolume;
            onSettingsChanged?.Invoke();
        }

        public void SetMusicVolume(float musicVolume)
        {
            this.musicVolume = musicVolume;
            onSettingsChanged?.Invoke();
        }

        public void SetSFXVolume(float sfxVolume)
        {
            this.sfxVolume = sfxVolume;
            onSettingsChanged?.Invoke();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            onSettingsChanged?.Invoke();
        }
#endif
    }
}
