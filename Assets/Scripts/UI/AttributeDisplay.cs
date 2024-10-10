using System;
using RPG.Attributes;
using TMPro;
using UnityEngine;

namespace RPG.UI
{
    public class AttributeDisplay : MonoBehaviour
    {
        [Tooltip("Component implementing <IAttributeProvider> interface")]
        [SerializeField] Behaviour valueProviderBehaviour;
        [SerializeField] ProgressBarData progressBarData = null;
        [SerializeField] DisplayTextData displayTextData = null;
        IAttributeProvider statsProvider;

        [System.Serializable]
        class ProgressBarData
        {
            public RectTransform foreground = null;
        }

        [System.Serializable]
        class DisplayTextData
        {
            public string label = null;
            public TMP_Text displayText = null;   
            public DisplayType displayType = DisplayType.CurrentInt;
        }

        enum DisplayType
        {
            CurrentInt,
            CurrentAndMaxInt,
            Currency,
            Time
        }

        void Awake()
        {
            statsProvider = valueProviderBehaviour as IAttributeProvider;
        }

        void Update()
        {
            if(displayTextData.displayText != null)
            {
                FillText();
            }

            if(progressBarData.foreground != null)
            {
                FillProgressBar();
            }
        }

        void FillText()
        {
            string label = displayTextData.label;

            switch(displayTextData.displayType)
            {
                case DisplayType.CurrentAndMaxInt:
                    displayTextData.displayText.text = label + string.Format("{0:0}/{1:0}", statsProvider.GetCurrentValue(), statsProvider.GetMaxValue());
                    break;
                case DisplayType.CurrentInt:
                    displayTextData.displayText.text = label + string.Format("{0:0}", statsProvider.GetCurrentValue());
                    break;
                case DisplayType.Currency:
                    displayTextData.displayText.text = label + $"${statsProvider.GetCurrentValue():N2}";
                    break;
                case DisplayType.Time:
                    string hour = DateTime.FromOADate(statsProvider.GetCurrentValue() * 1.0 / 24).
                        ToString(@"hh: mm tt", System.Globalization.CultureInfo.InvariantCulture).Replace(".", "");
                    hour = hour.Replace("AM", "am").Replace("PM", "pm");
                    displayTextData.displayText.text = label + hour;
                    break;

            }
        }

        void FillProgressBar()
        {
            progressBarData.foreground.localScale = new Vector3(GetFraction(), 1, 1);
        }

        float GetFraction()
        {
            return statsProvider.GetCurrentValue() / statsProvider.GetMaxValue();
        }
    }
}