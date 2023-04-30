namespace RPG.Attributes
{
    public interface IStatsProvider 
    {   
        float GetMaxValue();
        float GetCurrentValue();
    }
}
