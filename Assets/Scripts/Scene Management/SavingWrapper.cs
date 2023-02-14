using System.Collections;
using UnityEngine;
using GameDevTV.Saving;
using System;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        [SerializeField] private float fadeInTime = 0.2f;
        [SerializeField] float fadeOutTime = 0.2f;
        [SerializeField] int firstFieldBuildIndex = 1;

        const string currentSaveKey = "currentSaveFile";

        public void ContinueGame()
        {
            if(!CanContinue()) return;

            StartCoroutine(LoadLastScene());
        }

        public void NewGame(string saveFile)
        {
            if(string.IsNullOrEmpty(saveFile)) return;

            SetCurrentSave(saveFile);
            StartCoroutine(LoadFirstScene());
        }

        public void LoadGame(string saveFile)
        {
            if(!CanLoad()) return;

            SetCurrentSave(saveFile);
            ContinueGame();
        }

        public bool CanContinue()
        {
            if(!PlayerPrefs.HasKey(currentSaveKey)) return false;
            if(!GetComponent<SavingSystem>().SaveFileExists(GetCurrentSave())) return false;

            return true;
        }

        public bool CanLoad()
        {
            SavingSystem savingSystem = GetComponent<SavingSystem>();
            List<string> savingFiles = new List<string>(savingSystem.ListSaves());

            return savingFiles.Count > 0;
        }

        private void SetCurrentSave(string saveFile)
        {
            PlayerPrefs.SetString(currentSaveKey, saveFile);
        }

        private string GetCurrentSave()
        {
            return PlayerPrefs.GetString(currentSaveKey);
        }

        private IEnumerator LoadFirstScene()
        {
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeOutTime);
            yield return SceneManager.LoadSceneAsync(firstFieldBuildIndex);
            yield return fader.FadeIn(fadeInTime);
        }

        private IEnumerator LoadLastScene()
        {
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeOutTime);
            yield return GetComponent<SavingSystem>().LoadLastScene(GetCurrentSave());
            yield return fader.FadeIn(fadeInTime);
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }

            if(Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }

            if(Input.GetKeyDown(KeyCode.Delete))
            {
                Delete();
            }
        }

        public void Load()
        {
            GetComponent<SavingSystem>().Load(GetCurrentSave());
        }

        public void Save()
        {
            GetComponent<SavingSystem>().Save(GetCurrentSave());
        }

        private void Delete()
        {
            GetComponent<SavingSystem>().Delete(GetCurrentSave());
        }

        public IEnumerable<string> ListSaves()
        {
            return GetComponent<SavingSystem>().ListSaves();
        }
    }
}
