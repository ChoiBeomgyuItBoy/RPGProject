using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Saving;
using UnityEngine;
using UnityEngine.Audio;

namespace RPG.Audio
{
    public class AudioManager : MonoBehaviour, ISaveable
    {
        [SerializeField] AudioMixer audioMixer;
        [SerializeField] AudioSettingData[] audioSettingsData;
        Dictionary<AudioSetting, float> audioSettingLookup;
        public event Action onVolumeChange;

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
            onVolumeChange?.Invoke();
        }

        public Coroutine FadeOutMaster(float time)
        {
            return StartCoroutine(FadeSnapshot("FadeOutMaster", time));
        }

        public Coroutine FadeInMaster(float time)
        {
            return StartCoroutine(FadeSnapshot("FadeInMaster", time));
        }

        public Coroutine FadeOutMusic(float time)
        {
            return StartCoroutine(FadeSnapshot("FadeOutMusic", time));
        }

        public Coroutine FadeInMusic(float time)
        {
            return StartCoroutine(FadeSnapshot("FadeInMusic", time));
        }
        
        public IEnumerator FadeSnapshot(string snapshotName, float time)
        {
            var snapshot = audioMixer.FindSnapshot(snapshotName);

            if(snapshot == null)
            {
                Debug.LogError($"Snapshot '{snapshotName}' not found");
                yield break;
            }

            snapshot.TransitionTo(time);
            yield return new WaitForSeconds(time);
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

            onVolumeChange?.Invoke();
        }
    }
}
