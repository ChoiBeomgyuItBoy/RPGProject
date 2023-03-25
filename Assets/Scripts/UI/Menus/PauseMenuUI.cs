using RPG.Control;
using RPG.Core;
using RPG.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Menus
{
    public class PauseMenuUI : MonoBehaviour
    {
        [SerializeField] float normalTimeScale = 1;
        [SerializeField] Button saveButton;
        [SerializeField] Button saveAndQuitButton;
        PlayerController playerController;

        void Awake()
        {
            playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        }

        void Start()
        {
            saveButton.onClick.AddListener(Save);
            saveAndQuitButton.onClick.AddListener(SaveAndQuit);
        }

        void OnEnable()
        {
            if(playerController == null) return;

            Time.timeScale = 0;
            playerController.enabled = false;
        }

        void OnDisable()
        {
            if(playerController == null) return;
            
            Time.timeScale = normalTimeScale;
            playerController.enabled = true;
        }

        void Save()
        {
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
            savingWrapper.Save();
        }

        void SaveAndQuit()
        {
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
            savingWrapper.Save();
            savingWrapper.LoadMenu();
        }
    }
}

