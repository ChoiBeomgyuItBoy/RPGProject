using RPG.Settings;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Menus
{
    public class AudioMenuUI : MonoBehaviour
    {
        [SerializeField] AudioSettingsSO audioSettings;
        [SerializeField] Slider masterVolumeSlider;
        [SerializeField] Slider musicVolumeSlider;
        [SerializeField] Slider sfxVolumeSlider;

        void Start()
        {
            SetupSliders();

            masterVolumeSlider.onValueChanged.AddListener(audioSettings.SetMasterVolume);
            musicVolumeSlider.onValueChanged.AddListener(audioSettings.SetMusicVolume);
            sfxVolumeSlider.onValueChanged.AddListener(audioSettings.SetSFXVolume);
        }

        void SetupSliders()
        {
            masterVolumeSlider.value = audioSettings.GetMasterVolume();
            musicVolumeSlider.value = audioSettings.GetMusicVolume();
            sfxVolumeSlider.value = audioSettings.GetSFXVolume();
        }
    }
}
