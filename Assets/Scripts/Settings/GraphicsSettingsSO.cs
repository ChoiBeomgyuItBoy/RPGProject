using System.Collections.Generic;
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
        [SerializeField] Vector2Int[] resolutionProfiles;
        [SerializeField] [Range(-2, 0.5f)] float brightness = 0; 

        [Header("Graphics")]
        [SerializeField] VSyncMode vSyncMode = default;
        [SerializeField] UnityEngine.ShadowResolution shadowResolution = default;
        [SerializeField] AntialiasingMode antialiasingMode = default;
        [SerializeField] MotionBlurQuality motionBlurQuality = default;
        [SerializeField] BloomMode bloomMode = default;
        [SerializeField] VignetteMode vignetteMode = default;
        [SerializeField] DepthOfFieldMode depthOfFieldMode = default;
        [SerializeField] float vignetteMaxValue = 0.3f;

        Vector2Int currentResolution;

        public enum VSyncMode
        {
            Off = 0,
            On = 1
        }

        public enum BloomMode
        {
            Off = 0,
            Low = 3,
            Medium = 6,
            High = 10
        }

        public enum VignetteMode
        {
            Off = 0,
            On = 1
        }

        public FullScreenMode GetFullScreenMode() => fullScreenMode;
        public IEnumerable<Vector2Int> GetResolutionProfiles() => resolutionProfiles;
        public float GetBrightness() => brightness;
        public VSyncMode GetVSyncMode() => vSyncMode;
        public UnityEngine.ShadowResolution GetShadowResolution() => shadowResolution;
        public AntialiasingMode GetAntialiasingMode() => antialiasingMode;
        public MotionBlurQuality GetMotionBlurQuality() => motionBlurQuality;
        public BloomMode GetBloomMode() => bloomMode;
        public VignetteMode GetVignetteMode() => vignetteMode;
        public DepthOfFieldMode GetDepthOfFieldMode() => depthOfFieldMode;

        public void SetResolution(Vector2Int resolution)
        {
            currentResolution = resolution;
        }

        public void SetFullScreenMode(FullScreenMode fullScreenMode)
        {
            this.fullScreenMode = fullScreenMode;
        }

        public void SetBrightness(float brightness)
        {
            this.brightness = brightness;
        }

        public void SetVSyncMode(VSyncMode vSyncMode)
        {
            this.vSyncMode = vSyncMode;
        }

        public void SetShadowResolution(UnityEngine.ShadowResolution shadowResolution)
        {
            this.shadowResolution = shadowResolution;
        }

        public void SetAntialisingMode(AntialiasingMode antialiasingMode)
        {
            this.antialiasingMode = antialiasingMode;
        }

        public void SetMotionBlurQuality(MotionBlurQuality motionBlurQuality)
        {
            this.motionBlurQuality = motionBlurQuality;
        }

        public void SetBloomMode(BloomMode bloomMode)
        {
            this.bloomMode = bloomMode;
        }

        public void SetVignetteMode(VignetteMode vignetteMode)
        {
            this.vignetteMode = vignetteMode;
        }

        public void SetDepthOfFieldMode(DepthOfFieldMode depthOfFieldMode)
        {
            this.depthOfFieldMode = depthOfFieldMode;
        }

        public void ApplyChanges()
        {
            Screen.fullScreenMode = fullScreenMode;
            Screen.SetResolution(currentResolution.x, currentResolution.y, true);
            QualitySettings.vSyncCount = (int) vSyncMode;
            QualitySettings.shadowResolution = shadowResolution;
            QualitySettings.antiAliasing = (int) antialiasingMode;

            if(volumeProfile.TryGet(out ColorAdjustments colorAdjustments))
            {
                colorAdjustments.postExposure.value = brightness;
            }

            if(volumeProfile.TryGet(out MotionBlur motionBlur))
            {
                motionBlur.quality.value = motionBlurQuality;
            }

            if(volumeProfile.TryGet(out Bloom bloom))
            {
                bloom.intensity.value = (int) bloomMode;
            }

            if(volumeProfile.TryGet(out Vignette vignette))
            {
                vignette.intensity.value = vignetteMode == VignetteMode.Off? 0 : vignetteMaxValue;
            }

            if(volumeProfile.TryGet(out DepthOfField depthOfField))
            {
                depthOfField.mode.value = depthOfFieldMode;
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
