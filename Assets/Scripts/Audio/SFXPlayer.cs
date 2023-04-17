using UnityEngine;
using UnityEngine.Events;

namespace RPG.Audio
{
    public class SFXPlayer : SoundEmitter
    {
        [SerializeField] AudioConfig[] soundEffects;
        [SerializeField] UnityEvent onStart;

        public void Play(int index)
        {
            PlayAudio(soundEffects[index]);
        }

        public void PlayRandom()
        {
            Play(Random.Range(0, soundEffects.Length));
        }

        private void Start()
        {
            onStart?.Invoke();
        }
    }
}
