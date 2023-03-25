using RPG.Control;
using RPG.Core;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Menus
{
    public class SettingsMenuUI : MonoBehaviour
    {
        [SerializeField] GlobalSettings globalSettings;
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

            masterVolumeSlider.onValueChanged.AddListener(globalSettings.SetMasterVolume);
            musicVolumeSlider.onValueChanged.AddListener(globalSettings.SetMusicVolume);
            sfxVolumeSlider.onValueChanged.AddListener(globalSettings.SetSFXVolume);
        }

        void OnDisable()
        {
            masterVolumeSlider.onValueChanged.RemoveListener(globalSettings.SetMasterVolume);
            musicVolumeSlider.onValueChanged.RemoveListener(globalSettings.SetMusicVolume);
            sfxVolumeSlider.onValueChanged.RemoveListener(globalSettings.SetSFXVolume);
        }

        void SetupSliders()
        {
            masterVolumeSlider.value = globalSettings.GetMasterVolume();
            musicVolumeSlider.value = globalSettings.GetMusicVolume();
            sfxVolumeSlider.value = globalSettings.GetSFXVolume();
        }
    }
}
