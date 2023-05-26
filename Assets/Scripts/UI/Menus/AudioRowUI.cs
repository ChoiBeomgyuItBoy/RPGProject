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
        AudioManager audioManager;
        AudioSetting audioSetting;
        float volume;

        public void Setup(AudioManager audioManager, AudioSetting audioSetting, float volume)
        {
            this.audioManager = audioManager;
            this.audioSetting = audioSetting;
            this.volume = volume;
        }

        private void Start()
        {
            volumeSlider.value = audioManager.GetVolume(audioSetting);
            audioSettingText.text = $"{audioSetting.ToString()} volume";
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }

        private void SetVolume(float volume)
        {
            audioManager.SetVolume(audioSetting, volume);
        }
    }
}
