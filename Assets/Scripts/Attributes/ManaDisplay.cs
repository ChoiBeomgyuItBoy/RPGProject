using RPG.Attributes;
using TMPro;
using UnityEngine;

namespace RPG.Attributes
{
    public class ManaDisplay : MonoBehaviour
    {
        Mana mana;
        TMP_Text manaText;

        void Awake()
        {
            mana = GameObject.FindWithTag("Player").GetComponent<Mana>();
            manaText = GetComponent<TMP_Text>();
        }

        void Update()
        {
            manaText.text = string.Format("{0:0}/{1:0}", mana.GetMana(), mana.GetMaxMana());
        }
    }
}
