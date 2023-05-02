using UnityEngine;

namespace RPG.Core
{
    public class DayNightCycler : MonoBehaviour
    {
        [SerializeField] Material proceduralSkybox;
        [SerializeField] float maxDayLightIntensity = 2;
        Light mainLight = null;
        Clock clock = null;

        void Start()
        {
            clock = GameObject.FindWithTag("Player").GetComponent<Clock>();
            mainLight = GetComponent<Light>();
        }

        void Update()
        {
            float currentTime = clock.GetCurrentTime();

            if(currentTime >= 0 && currentTime < 6)
            {
                mainLight.intensity = 0;
                proceduralSkybox.SetFloat("_CubemapTransition", 1);
            }
            else if(currentTime >= 6 && currentTime <= 18)
            {
                float targetValue = ((currentTime - 6) / 12);
                mainLight.intensity = targetValue * maxDayLightIntensity;
                proceduralSkybox.SetFloat("_CubemapTransition", 1 - targetValue);
            }
            else
            {
                float targetValue = (currentTime - 18) / 6;
                mainLight.intensity = maxDayLightIntensity - (targetValue * maxDayLightIntensity);
                proceduralSkybox.SetFloat("_CubemapTransition", targetValue);
            }
        }
    }
}