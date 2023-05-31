namespace RPG.Graphics
{
    public enum VSyncMode
    {
        Off = 0,
        On = 1
    }

    public enum AntialiasingShortMode
    {
        None = 0,
        FXAA = 1,
        SMAA = 2
    }

    public enum BloomMode
    {
        Off = 0,
        Low = 1,
        Medium = 2,
        High = 3
    }
    
    public enum GraphicSetting
    {
        FullScreen,
        VSync,
        ShadowResolution,
        Antialiasing,
        Bloom,
        DepthOfField
    }
}
