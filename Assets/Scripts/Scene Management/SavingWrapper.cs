using System.Collections;
using UnityEngine;
using GameDevTV.Saving;
using System;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using RPG.Audio;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        [SerializeField] private float fadeInTime = 0.2f;
        [SerializeField] float fadeOutTime = 0.2f;
        [SerializeField] float fadeOutMusicTime = 5;
        [SerializeField] float fadeInMusicTime = 5;
        [SerializeField] int firstLevelBuildIndex = 1;
        [SerializeField] int menuLevelBuildIndex = 0;

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
            StartCoroutine(LoadScene(firstLevelBuildIndex));
        }

        public void LoadGame(string saveFile)
        {
            if(!CanLoad()) return;

            SetCurrentSave(saveFile);
            ContinueGame();
        }

        public void LoadMenu()
        {
            StartCoroutine(LoadScene(menuLevelBuildIndex));
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

        public IEnumerable<string> ListSaves()
        {
            return GetComponent<SavingSystem>().ListSaves();
        }

        private void SetCurrentSave(string saveFile)
        {
            PlayerPrefs.SetString(currentSaveKey, saveFile);
        }

        private string GetCurrentSave()
        {
            return PlayerPrefs.GetString(currentSaveKey);
        }

        private IEnumerator LoadScene(int sceneIndex)
        {
            Fader fader = FindObjectOfType<Fader>();
            AmbientAudioPlayer audioPlayer = FindObjectOfType<AmbientAudioPlayer>();

            yield return fader.FadeOut(fadeOutTime);
            yield return audioPlayer.FadeOutCurrentTrack(fadeOutMusicTime);
            yield return SceneManager.LoadSceneAsync(sceneIndex);
            yield return fader.FadeIn(fadeInTime);
            yield return audioPlayer.FadeTrack(TrackType.Ambient, sceneIndex, fadeInMusicTime);
        }

        private IEnumerator LoadLastScene()
        {
            Fader fader = FindObjectOfType<Fader>();
            AmbientAudioPlayer audioPlayer = FindObjectOfType<AmbientAudioPlayer>();
            SavingSystem savingSystem = GetComponent<SavingSystem>();

            yield return fader.FadeOut(fadeOutTime);
            yield return audioPlayer.FadeOutCurrentTrack(fadeOutMusicTime);
            yield return savingSystem.LoadLastScene(GetCurrentSave());
            yield return fader.FadeIn(fadeInTime);

            int sceneIndex = SceneManager.GetActiveScene().buildIndex;
            yield return audioPlayer.FadeTrack(TrackType.Ambient, sceneIndex, fadeInMusicTime);
        }

#if UNITY_EDITOR
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
#endif

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
    }
}
