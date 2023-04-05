using UnityEngine;

namespace RPG.Audio
{
    [System.Serializable]
    public class Track
    {
        [SerializeField] AudioClip clip;
        [SerializeField] [Range(0,1)] float volumeFraction = 0.2f;
        float resumeTime = 0;

        public AudioClip GetClip()
        {
            return clip;
        }

        public float GetVolumeFraction()
        {
            return volumeFraction;
        }

        public float GetResumeTime()
        {
            return resumeTime;
        }

        public void SetResumeTime(float resumeTime)
        {
            this.resumeTime = resumeTime;
        }
    }
}
