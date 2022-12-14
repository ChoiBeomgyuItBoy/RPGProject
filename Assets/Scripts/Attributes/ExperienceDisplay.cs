using UnityEngine;
using TMPro;
using RPG.Stats;

namespace RPG.Attributes
{
    public class ExperienceDisplay : MonoBehaviour
    {
        Experience experience;

        TMP_Text experienceValueText;

        void Awake()
        {
            experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
            experienceValueText = GetComponent<TMP_Text>();
        }

        void Update()
        {
            experienceValueText.text = string.Format("{0:0}", experience.GetPoints());
        }
    }
}

