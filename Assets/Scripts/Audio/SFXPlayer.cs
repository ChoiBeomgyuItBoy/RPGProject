using UnityEngine;

namespace RPG.Audio
{
    public class SFXPlayer : SoundEmitter
    {
        [SerializeField] AudioConfig[] soundEffects;
        [SerializeField] bool playRandomOnStart = false;

        public override void OnStartAction()
        {
            if(playRandomOnStart)
            {
                PlayRandom();
            }
        }

        public void Play(int index)
        {
            PlayAudio(soundEffects[index]);
        }

        public void PlayRandom()
        {
            Play(Random.Range(0, soundEffects.Length));
        }
    }
}
