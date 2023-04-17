using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace RPG.Audio
{
    public class AudioFader : MonoBehaviour
    {
        [SerializeField] AudioMixer audioMixer;

        public Coroutine FadeOutMaster(float time)
        {
            return StartCoroutine(FadeSnapshot("FadeOutMaster", time));
        }

        public Coroutine FadeInMaster(float time)
        {
            return StartCoroutine(FadeSnapshot("FadeInMaster", time));
        }

        public Coroutine FadeOutMusic(float time)
        {
            return StartCoroutine(FadeSnapshot("FadeOutMusic", time));
        }

        public Coroutine FadeInMusic(float time)
        {
            return StartCoroutine(FadeSnapshot("FadeInMusic", time));
        }

        public Coroutine FadeMusicLowVolume(float time)
        {
            return StartCoroutine(FadeSnapshot("FadeMusicLowVolume", time));
        }

        private IEnumerator FadeSnapshot(string snapshotName, float time)
        {
            var snapshot = audioMixer.FindSnapshot(snapshotName);

            if(snapshot == null)
            {
                Debug.LogError($"Snapshot '{snapshotName}' not found");
                yield break;
            }

            snapshot.TransitionTo(time);
            yield return new WaitForSeconds(time);
        }
    }   
}
