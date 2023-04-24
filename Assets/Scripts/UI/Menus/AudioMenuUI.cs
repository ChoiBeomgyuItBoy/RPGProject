using RPG.Core;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Menus
{
    public class AudioMenuUI : MonoBehaviour
    {
        [SerializeField] SettingsMenu settings;
        [SerializeField] Slider masterVolumeSlider;
        [SerializeField] Slider musicVolumeSlider;
        [SerializeField] Slider sfxVolumeSlider;

        void Start()
        {
            SetupSliders();

            masterVolumeSlider.onValueChanged.AddListener(settings.SetMasterVolume);
            musicVolumeSlider.onValueChanged.AddListener(settings.SetMusicVolume);
            sfxVolumeSlider.onValueChanged.AddListener(settings.SetSFXVolume);
        }

        void SetupSliders()
        {
            masterVolumeSlider.value = settings.GetMasterVolume();
            musicVolumeSlider.value = settings.GetMusicVolume();
            sfxVolumeSlider.value = settings.GetSFXVolume();
        }
    }
}
