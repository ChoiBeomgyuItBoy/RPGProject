using System;
using UnityEngine;

namespace RPG.Core
{
    [CreateAssetMenu(menuName = "RPG/Settings/New Player Settings")]
    public class PlayerSettings : ScriptableObject
    {  
        [Header("Audio")]
        [SerializeField] [Range(0,1)] float masterVolume = 1;
        [SerializeField] [Range(0,1)] float musicVolume = 1;
        [SerializeField] [Range(0,1)] float sfxVolume = 1;

        [Header("Key Bindings")]
        [SerializeField] KeyCode movementKey = KeyCode.Mouse0;
        [SerializeField] KeyCode interactionKey = KeyCode.Mouse0;
        [SerializeField] KeyCode inventoryKey = KeyCode.I;
        [SerializeField] KeyCode questsKey = KeyCode.Q;
        [SerializeField] KeyCode traitsKey = KeyCode.T;
        [SerializeField] KeyCode pauseKey = KeyCode.Escape;

        public event Action onSettingsChanged;

        public float GetMasterVolume() => masterVolume;
        public float GetMusicVolume() => musicVolume;
        public float GetSFXVolume() => sfxVolume;

        public KeyCode GetMovementKey() => movementKey;
        public KeyCode GetInteractionKey() => interactionKey;
        public KeyCode GetInventoryKey() => inventoryKey;
        public KeyCode GetQuestsKey() => questsKey;
        public KeyCode GetTraitsKey() => traitsKey;
        public KeyCode GetPauseKey() => pauseKey;

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
