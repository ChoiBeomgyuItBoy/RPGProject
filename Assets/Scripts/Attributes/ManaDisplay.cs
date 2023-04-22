using TMPro;
using UnityEngine;

namespace RPG.Attributes
{
    public class ManaDisplay : MonoBehaviour
    {
        [SerializeField] float colorChangeTreshold = 40;
        Mana mana;
        TMP_Text manaText;
        Color defaultTextColor;

        void Awake()
        {
            mana = GameObject.FindWithTag("Player").GetComponent<Mana>();
            manaText = GetComponent<TMP_Text>();
            defaultTextColor = manaText.color;
        }

        void Update()
        {
            manaText.color = mana.GetMana() < colorChangeTreshold? Color.red : defaultTextColor;
            manaText.text = string.Format("{0:0}/{1:0}", mana.GetMana(), mana.GetMaxMana());
        }
    }
}
