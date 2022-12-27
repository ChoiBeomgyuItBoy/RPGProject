using UnityEngine;
using TMPro;
using RPG.Attributes;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Fighter fighter;

        TMP_Text healthValueText;

        void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
            healthValueText = GetComponent<TMP_Text>();
        }

        void Update()
        {
            if(fighter.GetTarget() == null)
            {
                healthValueText.text = "N/A";
                return;
            }

            Health targetHealth = fighter.GetTarget();

            healthValueText.text = string.Format("{0:0}/{1:0}", targetHealth.GetCurrentHealth(), targetHealth.GetMaxHealth());
        }
    }   
}
