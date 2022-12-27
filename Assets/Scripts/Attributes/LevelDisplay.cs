using UnityEngine;
using TMPro;
using RPG.Stats;

namespace RPG.Attributes
{
    public class LevelDisplay : MonoBehaviour
    {
        BaseStats stats;

        TMP_Text experienceValueText;

        void Awake()
        {
            stats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
            experienceValueText = GetComponent<TMP_Text>();
        }

        void Update()
        {
            experienceValueText.text = string.Format("{0:0}", stats.GetLevel());
        }
    }
}
