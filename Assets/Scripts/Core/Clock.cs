using GameDevTV.Saving;
using RPG.Attributes;
using UnityEngine;

namespace RPG.Core
{   
    public class Clock : MonoBehaviour, ISaveable, IAttributeProvider
    {
        [SerializeField] float timeScale = 1;
        [SerializeField] float currentTime = 0;

        public float GetCurrentTime()
        {
            return currentTime;
        }

        void Update()                                                                                      
        {   
            UpdateTime();
        }  

        void UpdateTime()
        {
            currentTime += Time.deltaTime * timeScale;

            if(currentTime >= 24)
            {
                currentTime -= 24;
            }
        }

        object ISaveable.CaptureState()
        {
            return currentTime;
        }

        void ISaveable.RestoreState(object state)
        {
            currentTime = (float) state;
        }

        float IAttributeProvider.GetMaxValue()
        {
            return 0;
        }

        float IAttributeProvider.GetCurrentValue()
        {
            return currentTime;
        }
    }
}
