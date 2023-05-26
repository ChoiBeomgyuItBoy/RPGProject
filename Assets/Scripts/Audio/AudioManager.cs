using System;
using System.Collections.Generic;
using GameDevTV.Saving;
using UnityEngine;

namespace RPG.Audio
{
    public class AudioManager : MonoBehaviour, ISaveable
    {
        [SerializeField] AudioSettingData[] audioSettingsData;
        Dictionary<AudioSetting, float> audioSettingLookup;
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

            return audioSettingLookup[audioSetting];
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
            onChange?.Invoke();
        }

        [System.Serializable]
        class AudioSettingData
        {
            public AudioSetting audioSetting;
            [Range(0,1)] public float volume;
        }

        void BuildLookup()
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

        object ISaveable.CaptureState()
        {
            Dictionary<int, float> saveObject = new Dictionary<int, float>();

            foreach(var pair in audioSettingLookup)
            {
                saveObject[(int) pair.Key] = pair.Value;
            }

            return saveObject;
        }

        void ISaveable.RestoreState(object state)
        {
            Dictionary<int, float> saveObject = (Dictionary<int, float>) state;

            foreach(var pair in saveObject)
            {
                audioSettingLookup[(AudioSetting) pair.Key] = pair.Value;
            }

            onChange?.Invoke();
        }
    }
}
