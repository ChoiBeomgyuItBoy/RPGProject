using UnityEngine;
using TMPro;

namespace RPG.Attributes
{
    public class ExperienceDisplay : MonoBehaviour
    {
        Experience experience;

        TMP_Text healthValueText;

        void Awake()
        {
            experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
            healthValueText = GetComponent<TMP_Text>();
        }

        void Update()
        {
            healthValueText.text = string.Format("{0:0.0}", experience.GetPoints());
        }
    }
}

