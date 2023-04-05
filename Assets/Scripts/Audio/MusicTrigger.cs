using UnityEngine;

namespace RPG.Audio
{
    public class MusicTrigger : MusicFader
    {
        [SerializeField] Track[] tracks;

        // Use for Unity Events
        public void PlayMusic(int trackIndex)
        {
            Track selectedTrack = tracks[trackIndex];
            StartCoroutine(FadeOutInMusic(selectedTrack));
        }
    }
}
