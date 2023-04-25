using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace RPG.Settings
{
    [CreateAssetMenu(menuName = "RPG/Settings/New Graphics Settings")]
    public class GraphicsSettingsSO : ScriptableObject
    {
        [SerializeField] VolumeProfile volumeProfile;
        
        [Header("Screen")]
        [SerializeField] FullScreenMode fullScreenMode = FullScreenMode.FullScreenWindow;
        [SerializeField] Vector2[] resolutionProfiles;
        [SerializeField] [Range(-2, 0.5f)] float brightness = 0; 

        [Header("Graphics")]
        [SerializeField] Mode vSync = default;
        [SerializeField] UnityEngine.ShadowResolution shadowResolution = default;
        [SerializeField] AntiAliasingMode antiAliasingMode = default;
        [SerializeField] MotionBlurQuality motionBlurQuality = default;
        [SerializeField] Quality bloom = default;
        [SerializeField] Mode vignette = default;
        [SerializeField] DepthOfFieldMode depthOfFieldMode = default;

        enum Mode
        {
            On, Off
        }

        enum Quality
        {
            High, Medium, Low
        }

        enum AntiAliasingMode
        {
            None, TAA, MSAA
        }

        void ApplyChanges()
        {
            SetScreenSettings();
            SetBrightness();
            SetVignette();
            SetDepthOfField();
            SetMotionBlur();
            SetBloom();
        }

        void SetScreenSettings()
        {
            Screen.fullScreenMode = fullScreenMode;
            QualitySettings.shadowResolution = shadowResolution;
            QualitySettings.vSyncCount = vSync == Mode.On? 1 : 0;
        }

        void SetBrightness()
        {
            if(volumeProfile.TryGet(out ColorAdjustments colorAdjustments))
            {
                colorAdjustments.postExposure.value = brightness;
            }
        }

        void SetVignette()
        {
            if(volumeProfile.TryGet(out Vignette vignette))
            {
                if(this.vignette == Mode.Off)
                {
                    vignette.intensity.value = 0;
                }
                if(this.vignette == Mode.On)
                {
                    vignette.intensity.value = 0.5f;
                }
            }
        }

        void SetAntiAliasing()
        {
            if(antiAliasingMode == AntiAliasingMode.None)
            {
                QualitySettings.antiAliasing = 0;
            }
            if(antiAliasingMode == AntiAliasingMode.TAA)
            {
                QualitySettings.antiAliasing = 1;
            }
            if(antiAliasingMode == AntiAliasingMode.MSAA)
            {
                QualitySettings.antiAliasing = 4;
            }
        }

        void SetDepthOfField()
        {
            if(volumeProfile.TryGet(out DepthOfField depthOfField))
            {
                depthOfField.mode = new DepthOfFieldModeParameter(depthOfFieldMode);
            }
        }

        void SetMotionBlur()
        {
            if(volumeProfile.TryGet(out MotionBlur motionBlur))
            {
                motionBlur.quality = new MotionBlurQualityParameter(motionBlurQuality);
            }
        }

        void SetBloom()
        {
            if(volumeProfile.TryGet(out Bloom bloom))
            {
                float bloomIntensity = 0;

                if(this.bloom == Quality.High)
                {
                    bloomIntensity = 10;
                }
                if(this.bloom == Quality.Medium)
                {
                    bloomIntensity = 6;
                }
                if(this.bloom == Quality.Low)
                {
                    bloomIntensity = 3;
                }
                bloom.intensity.value = bloomIntensity;
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            ApplyChanges();
        }
#endif
    } 
}
