using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using RPG.Core;
using RPG.Control;
using RPG.Audio;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour, IRaycastable
    {
        enum DestinationIdentifier
        {
            A, B, C, D, E
        }

        [SerializeField] int sceneToLoad;
        [SerializeField] float fadeOutTime = 0.5f;
        [SerializeField] float fadeInTime = 1f;
        [SerializeField] float fadeWaitTime = 0.5f;
        [SerializeField] float fadeOutMusicTime = 8;
        [SerializeField] float fadeInMusicTime = 5;
        [SerializeField] Transform spawnPoint;
        [SerializeField] DestinationIdentifier destination;
        [SerializeField] bool raycastable = false;

        void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Player")
            {
                StartCoroutine(Transition());
            }
        }

        IEnumerator Transition()
        {
            if(sceneToLoad < 0) 
            { 
                Debug.LogError("Scene to load not set"); 
                yield break;
            }

            DontDestroyOnLoad(gameObject);

            Fader fader = FindObjectOfType<Fader>();
            SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
            AmbientAudioPlayer audioPlayer = FindObjectOfType<AmbientAudioPlayer>();

            ToggleControl(false);

            yield return fader.FadeOut(fadeOutTime);
            yield return audioPlayer.FadeOutCurrentTrack(fadeOutMusicTime);

            wrapper.Save();

            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            
            ToggleControl(false);

            wrapper.Load();

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            wrapper.Save();

            yield return new WaitForSeconds(fadeWaitTime);
            fader.FadeIn(fadeInTime);

            yield return audioPlayer.FadeTrack(TrackType.Ambient, sceneToLoad, fadeInMusicTime);

            ToggleControl(true);

            Destroy(gameObject);
        }


        Portal GetOtherPortal()
        {
            foreach(Portal portal in FindObjectsOfType<Portal>())
            {
                if(portal == this) continue;

                if(portal.destination != destination) continue;

                return portal;
            }

            return null;
        }

        void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");

            player.GetComponent<NavMeshAgent>().enabled = false;

            player.transform.position = otherPortal.spawnPoint.position;
            player.transform.rotation = otherPortal.spawnPoint.rotation;

            player.GetComponent<NavMeshAgent>().enabled = true;
        }

        void ToggleControl(bool state)
        {
            GameObject player = GameObject.FindWithTag("Player");

            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<PlayerController>().enabled = state;
        }

        bool IRaycastable.HandleRaycast(PlayerController callingController)
        {
            if(!raycastable) return false;

            if(Input.GetMouseButtonDown(0))
            {
                StartCoroutine(Transition());
            }

            return true;
        }

        CursorType IRaycastable.GetCursorType()
        {
            return CursorType.Door;
        }
    }
}

