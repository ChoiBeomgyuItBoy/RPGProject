using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Audio
{
    [CreateAssetMenu(menuName = "RPG/Settings/New Audio Settings")]
    public class AudioSettingsSO : ScriptableObject
    {  
        [SerializeField] AudioSettingData[] audioSettingsData;
        Dictionary<AudioSetting, float> audioSettingLookup;

        [System.Serializable]
        class AudioSettingData
        {
            public AudioSetting audioSetting;
            [Range(0,1)] public float volume;
        }

        public event Action onChange;

        public IEnumerable<KeyValuePair<AudioSetting, float>> GetAudioPair()
        {
            return audioSettingLookup;
        }

        public float GetVolume(AudioSetting audioSetting)
        {
            if(audioSettingLookup == null)
            {
                BuildLookup();
            }

            if(!audioSettingLookup.ContainsKey(audioSetting))
            {
                Debug.LogError($"Audio Data for {audioSetting} not found");
                return 0;
            }

            return audioSettingLookup[audioSetting] * audioSettingLookup[AudioSetting.Master];
        }

        public void SetVolume(AudioSetting audioSetting, float volume)
        {
            if(audioSettingLookup == null)
            {
                BuildLookup();
            }

            if(!audioSettingLookup.ContainsKey(audioSetting))
            {
                Debug.LogError($"Audio Data for {audioSetting} not found");
                return;
            }

            audioSettingLookup[audioSetting] = volume;
            SaveChanges();
            onChange?.Invoke();
        }

        private void BuildLookup()
        {
            audioSettingLookup = new Dictionary<AudioSetting, float>();
            foreach(var settingData in audioSettingsData)
            {
                if(audioSettingLookup.ContainsKey(settingData.audioSetting))
                {
                    Debug.LogError($"Duplicated Audio Data for {settingData.audioSetting}");
                }

                audioSettingLookup[settingData.audioSetting] = settingData.volume;
            }
        }

        private void SaveChanges()
        {
            for (int i = 0; i < audioSettingsData.Length; i++)
            {
                AudioSettingData selectedData = audioSettingsData[i];
                selectedData.volume = audioSettingLookup[selectedData.audioSetting];
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            onChange?.Invoke();
        }
#endif
    }
}
