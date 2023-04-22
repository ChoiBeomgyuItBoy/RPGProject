using UnityEngine;
using TMPro;
using RPG.Stats;

namespace RPG.Attributes
{
    public class LevelDisplay : MonoBehaviour
    {
        [SerializeField] BaseStats baseStats;
        TMP_Text experienceValueText;

        void Awake()
        {
            experienceValueText = GetComponent<TMP_Text>();
        }

        void Update()
        {
            experienceValueText.text = string.Format("{0:0}", baseStats.GetLevel());
        }
    }
}
