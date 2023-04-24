using RPG.Control;
using RPG.Dialogue;
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
        [SerializeField] Button quitButton;

        void Start()
        {
            Time.timeScale = normalTimeScale;
            saveButton.onClick.AddListener(Save);
            saveAndQuitButton.onClick.AddListener(SaveAndQuit);
            quitButton.onClick.AddListener(Quit);
        }

        void OnEnable()
        {
            Time.timeScale = 0;
        }

        void OnDisable()
        {
            Time.timeScale = normalTimeScale;
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

        void Quit()
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
    }
}

