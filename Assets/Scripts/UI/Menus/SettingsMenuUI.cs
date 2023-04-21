using RPG.Control;
using RPG.Core;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Menus
{
    public class SettingsMenuUI : MonoBehaviour
    {
        [SerializeField] SettingsMenu settings;
        [SerializeField] Slider masterVolumeSlider;
        [SerializeField] Slider musicVolumeSlider;
        [SerializeField] Slider sfxVolumeSlider;

        PlayerController playerController;

        void Awake()
        {
            playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        }

        void OnEnable()
        {
            SetupSliders();

            masterVolumeSlider.onValueChanged.AddListener(settings.SetMasterVolume);
            musicVolumeSlider.onValueChanged.AddListener(settings.SetMusicVolume);
            sfxVolumeSlider.onValueChanged.AddListener(settings.SetSFXVolume);
        }

        void OnDisable()
        {
            masterVolumeSlider.onValueChanged.RemoveListener(settings.SetMasterVolume);
            musicVolumeSlider.onValueChanged.RemoveListener(settings.SetMusicVolume);
            sfxVolumeSlider.onValueChanged.RemoveListener(settings.SetSFXVolume);
        }

        void SetupSliders()
        {
            masterVolumeSlider.value = settings.GetMasterVolume();
            musicVolumeSlider.value = settings.GetMusicVolume();
            sfxVolumeSlider.value = settings.GetSFXVolume();
        }
    }
}
