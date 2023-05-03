using UnityEngine;

namespace RPG.TimeManagement
{
    public class DayNightCycler : MonoBehaviour
    {
        [SerializeField] Material proceduralSkybox;
        [SerializeField] float maxDayLightIntensity = 2;
        [SerializeField] float latitude = 45;
        Light mainLight = null;
        Clock clock = null;

        void Start()
        {
            clock = GameObject.FindWithTag("Player").GetComponent<Clock>();
            mainLight = GetComponent<Light>();
        }

        void Update()
        {
            UpdateSky();
            UpdateSunRotation();
        }

        void UpdateSky()
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

        void UpdateSunRotation()
        {
            float currentTime = clock.GetCurrentTime();
            float sunAngle = (currentTime / 24) * 360 - 90;
            float latitudeRad = latitude * Mathf.Deg2Rad;
            float sunX = Mathf.Cos(sunAngle * Mathf.Deg2Rad) * Mathf.Cos(latitudeRad);
            float sunY = Mathf.Sin(latitudeRad);
            float sunZ = Mathf.Sin(sunAngle * Mathf.Deg2Rad) * Mathf.Cos(latitudeRad);
            Vector3 sunDirection = new Vector3(sunX, sunY, sunZ);
            mainLight.transform.rotation = Quaternion.LookRotation(sunDirection);
        }
    }
}