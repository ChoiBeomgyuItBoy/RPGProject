using GameDevTV.Saving;
using GameDevTV.Utils;
using RPG.Attributes;
using UnityEngine;

namespace RPG.Core
{   
    public class Clock : MonoBehaviour, ISaveable, IAttributeProvider, IPredicateEvaluator
    {
        [SerializeField] float timeScale = 1;
        [SerializeField] [Range(0,24)] float initialTime = 6;
        float currentTime = 0;

        public float GetCurrentTime()
        {
            return currentTime;
        }

        void Awake()
        {
            currentTime = initialTime;
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

        bool? IPredicateEvaluator.Evaluate(string predicate, string[] parameters)
        {
            switch(predicate)
            {
                case "IsMorning":
                    return currentTime >= 6 && currentTime < 12;
                case "IsEvening":
                    return currentTime >= 12 && currentTime < 19;
                case "IsNight":
                    return currentTime >= 19 || currentTime < 6;
            }

            return null;
        }
    }
}
