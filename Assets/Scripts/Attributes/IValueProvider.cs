using System;

namespace RPG.Attributes
{
    public interface IValueProvider 
    {   
        float GetMaxValue();
        float GetCurrentValue();
    }
}
