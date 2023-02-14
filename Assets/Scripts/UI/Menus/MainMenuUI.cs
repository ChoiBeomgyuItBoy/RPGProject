using GameDevTV.Utils;
using RPG.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace RPG.UI.Menus
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] Button continueButton;
        [SerializeField] Button newGameButton;
        [SerializeField] Button loadButton;
        [SerializeField] Button quitButton;
        [SerializeField] TMP_InputField newGameInput;
        LazyValue<SavingWrapper> savingWrapper;

        void Awake()
        {
            savingWrapper = new LazyValue<SavingWrapper>(GetSavingWrapper);
        }

        void Start()
        {
            savingWrapper.ForceInit();
            continueButton.onClick.AddListener(ContinueGame);
            newGameButton.onClick.AddListener(NewGame);
            quitButton.onClick.AddListener(Quit);

            continueButton.interactable = savingWrapper.value.CanContinue();
            loadButton.interactable = savingWrapper.value.CanLoad();
        }

        SavingWrapper GetSavingWrapper()
        {
            return FindObjectOfType<SavingWrapper>();
        }

        void ContinueGame()
        {
            savingWrapper.value.ContinueGame();
        }

        void NewGame()
        {
            savingWrapper.value.NewGame(newGameInput.text);
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

