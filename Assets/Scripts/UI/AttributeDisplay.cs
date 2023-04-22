using RPG.Attributes;
using TMPro;
using UnityEngine;

namespace RPG.UI
{
    public class AttributeDisplay : MonoBehaviour
    {
        [Tooltip("Component implementing <IValueProvider> interface")]
        [SerializeField] Behaviour valueProviderBehaviour;
        [SerializeField] ProgressBarData progressBarData = null;
        [SerializeField] DisplayTextData displayTextData = null;
        IValueProvider valueProvider;
        float initalValue = 0;

        [System.Serializable]
        class ProgressBarData
        {
            public RectTransform foreground = null;
        }

        [System.Serializable]
        class DisplayTextData
        {
            public TMP_Text displayText = null;   
            public bool displayAsCurrency = false;
            public bool displayCurrentAndMaxValues = true;
        }

        void Awake()
        {
            valueProvider = valueProviderBehaviour as IValueProvider;
        }

        void Start()
        {
            initalValue = valueProvider.GetCurrentValue();
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
            if(displayTextData.displayCurrentAndMaxValues)
            {
                displayTextData.displayText.text = string.Format("{0:0}/{1:0}", valueProvider.GetCurrentValue(), valueProvider.GetMaxValue());
            }
            else
            {
                displayTextData.displayText.text = string.Format("{0:0}", valueProvider.GetCurrentValue());
            }

            if(displayTextData.displayAsCurrency)
            {
                displayTextData.displayText.text = $"${valueProvider.GetCurrentValue():N2}";
            }
            

        }

        void FillProgressBar()
        {
            progressBarData.foreground.localScale = new Vector3(GetFraction(), 1, 1);
        }

        float GetFraction()
        {
            return valueProvider.GetCurrentValue() / valueProvider.GetMaxValue();
        }
    }
}