using RPG.Control;
using RPG.Core;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Menus
{
    public class SettingsMenuUI : MonoBehaviour
    {
        [SerializeField] PlayerSettings playerSettings;
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

            masterVolumeSlider.onValueChanged.AddListener(playerSettings.SetMasterVolume);
            musicVolumeSlider.onValueChanged.AddListener(playerSettings.SetMusicVolume);
            sfxVolumeSlider.onValueChanged.AddListener(playerSettings.SetSFXVolume);
        }

        void OnDisable()
        {
            masterVolumeSlider.onValueChanged.RemoveListener(playerSettings.SetMasterVolume);
            musicVolumeSlider.onValueChanged.RemoveListener(playerSettings.SetMusicVolume);
            sfxVolumeSlider.onValueChanged.RemoveListener(playerSettings.SetSFXVolume);
        }

        void SetupSliders()
        {
            masterVolumeSlider.value = playerSettings.GetMasterVolume();
            musicVolumeSlider.value = playerSettings.GetMusicVolume();
            sfxVolumeSlider.value = playerSettings.GetSFXVolume();
        }
    }
}
