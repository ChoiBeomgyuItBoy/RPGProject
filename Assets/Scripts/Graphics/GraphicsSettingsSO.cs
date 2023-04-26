using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace RPG.Graphics
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
        [SerializeField] bool useMotionBlur = false;
        [SerializeField] float vignetteMaxValue = 0.3f;
        [SerializeField] Vector2 currentResolution;

        int currentResolutionIndex;

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
        public float GetBrightness() => brightness;
        public VSyncMode GetVSyncMode() => vSyncMode;
        public UnityEngine.ShadowResolution GetShadowResolution() => shadowResolution;
        public AntialiasingMode GetAntialiasingMode() => antialiasingMode;
        public MotionBlurQuality GetMotionBlurQuality() => motionBlurQuality;
        public BloomMode GetBloomMode() => bloomMode;
        public VignetteMode GetVignetteMode() => vignetteMode;
        public DepthOfFieldMode GetDepthOfFieldMode() => depthOfFieldMode;
        
        public IEnumerable<string> GetResolutionProfiles()
        {
            foreach(var resolution in resolutionProfiles)
            {
                yield return $"{resolution.x} x {resolution.y}";
            }
        }

        public int GetCurrentResolutionProfile()
        {
            return currentResolutionIndex;
        }

        public void SetResolution(int resolutionProfileIndex)
        {
            currentResolution = resolutionProfiles[resolutionProfileIndex];
            currentResolutionIndex = resolutionProfileIndex;
            Screen.SetResolution(resolutionProfiles[currentResolutionIndex].x, resolutionProfiles[currentResolutionIndex].y, true);
        }

        public void SetFullScreenMode(int option)
        {
            this.fullScreenMode = (FullScreenMode) option;
            Screen.fullScreenMode = fullScreenMode;
        }

        public void SetVSyncMode(int option)
        {
            this.vSyncMode = (VSyncMode) option;
            QualitySettings.vSyncCount = (int) vSyncMode;
        }

        public void SetShadowResolution(int option)
        {
            this.shadowResolution = (UnityEngine.ShadowResolution) option;
            QualitySettings.shadowResolution = shadowResolution;
        }

        public void SetAntialisingMode(int option)
        {
            this.antialiasingMode = (AntialiasingMode) option;
            QualitySettings.antiAliasing = (int) antialiasingMode;
        }

        public void SetMotionBlurQuality(int option)
        {
            this.motionBlurQuality = (MotionBlurQuality) option;

            if(volumeProfile.TryGet(out MotionBlur motionBlur))
            {
                motionBlur.active = useMotionBlur;
                motionBlur.quality.value = motionBlurQuality;
            }
        }
        
        public void SetBrightness(float brightness)
        {
            this.brightness = brightness;

            if(volumeProfile.TryGet(out ColorAdjustments colorAdjustments))
            {
                colorAdjustments.postExposure.value = brightness;
            }
        }

        public void SetBloomMode(int option)
        {
            this.bloomMode = (BloomMode) option;

            if(volumeProfile.TryGet(out Bloom bloom))
            {
                bloom.intensity.value = (int) bloomMode;
            }
        }

        public void SetVignetteMode(int option)
        {
            this.vignetteMode = (VignetteMode) option;

            if(volumeProfile.TryGet(out Vignette vignette))
            {
                vignette.intensity.value = vignetteMode == VignetteMode.Off? 0 : vignetteMaxValue;
            }
        }

        public void SetDepthOfFieldMode(int option)
        {
            this.depthOfFieldMode = (DepthOfFieldMode) option;

            if(volumeProfile.TryGet(out DepthOfField depthOfField))
            {
                depthOfField.mode.value = depthOfFieldMode;
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            SetVSyncMode((int) vSyncMode);
            SetShadowResolution((int) shadowResolution);
            SetAntialisingMode((int) antialiasingMode);
            SetMotionBlurQuality((int) motionBlurQuality);
            SetBrightness(brightness);
            SetBloomMode((int) bloomMode);
            SetVignetteMode((int) vignetteMode);
            SetDepthOfFieldMode((int) depthOfFieldMode);
        }
#endif
    } 
}
