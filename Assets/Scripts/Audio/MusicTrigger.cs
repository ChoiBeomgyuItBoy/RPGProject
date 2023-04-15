using UnityEngine;

namespace RPG.Audio
{
    public class MusicTrigger : MusicFader
    {
        [SerializeField] Track[] tracks;

        public void PlayMusic(int trackIndex)
        {
            Track selectedTrack = tracks[trackIndex];
            StartCoroutine(FadeOutInMusic(selectedTrack, false));
        }

        public void PlayMusicLowVolume(int trackIndex)
        {
            Track selectedTrack = tracks[trackIndex];
            StartCoroutine(FadeOutInMusic(selectedTrack, true));
        }
    }
}
