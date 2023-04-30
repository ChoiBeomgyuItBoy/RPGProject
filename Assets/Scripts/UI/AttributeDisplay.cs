using RPG.Attributes;
using TMPro;
using UnityEngine;

namespace RPG.UI
{
    public class AttributeDisplay : MonoBehaviour
    {
        [Tooltip("Component implementing <IStatsProvider> interface")]
        [SerializeField] Behaviour valueProviderBehaviour;
        [SerializeField] ProgressBarData progressBarData = null;
        [SerializeField] DisplayTextData displayTextData = null;
        IStatsProvider statsProvider;

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
            statsProvider = valueProviderBehaviour as IStatsProvider;
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
                displayTextData.displayText.text = string.Format("{0:0}/{1:0}", statsProvider.GetCurrentValue(), statsProvider.GetMaxValue());
            }
            else
            {
                displayTextData.displayText.text = string.Format("{0:0}", statsProvider.GetCurrentValue());
            }

            if(displayTextData.displayAsCurrency)
            {
                displayTextData.displayText.text = $"${statsProvider.GetCurrentValue():N2}";
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