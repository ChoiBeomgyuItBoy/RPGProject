using UnityEngine;

namespace RPG.Audio
{
    [System.Serializable]
    public class AudioConfig 
    {   
        [SerializeField] AudioClip clip;
        [SerializeField] [Range(0,1)] float volumeFraction = 1;
        [SerializeField] bool hasToLoop = false;
        [SerializeField] bool hasToResume = false;
        float resumeTime = 0;

        public AudioClip GetClip()
        {
            return clip;
        }

        public float GetVolumeFraction()
        {
            return volumeFraction;
        }

        public bool HasToLoop()
        {
            return hasToLoop;
        }

        public bool HasToResume()
        {
            return hasToResume;
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