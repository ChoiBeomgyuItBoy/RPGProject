using RPG.Audio;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Menus
{
    public class AudioRowUI : MonoBehaviour
    {
        [SerializeField] Slider volumeSlider;
        [SerializeField] TMP_Text audioSettingText;
        AudioSettingsSO audioSettingsSO;
        AudioSetting audioSetting;
        float volume;

        public void Setup(AudioSettingsSO audioSettingsSO, AudioSetting audioSetting, float volume)
        {
            this.audioSettingsSO = audioSettingsSO;
            this.audioSetting = audioSetting;
            this.volume = volume;
        }

        private void Start()
        {
            SetVolume(audioSettingsSO.GetVolume(audioSetting));
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }

        private void SetVolume(float volume)
        {
            audioSettingsSO.SetVolume(audioSetting, volume);
            volumeSlider.value = volume;
            audioSettingText.text = $"{audioSetting.ToString()} volume";
        }
    }
}
