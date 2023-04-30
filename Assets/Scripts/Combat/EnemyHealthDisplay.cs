using UnityEngine;
using TMPro;
using RPG.Attributes;
using RPG.Stats;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        [SerializeField] TMP_Text enemyClassText;
        [SerializeField] TMP_Text healthValueText;
        Fighter fighter;

        void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        void Update()
        {
            if(fighter.GetTarget() == null)
            {
                enemyClassText.enabled = false;
                healthValueText.enabled = false;
                return;
            }


            enemyClassText.enabled = true;
            healthValueText.enabled = true;
            Health targetHealth = fighter.GetTarget();
            healthValueText.text = string.Format("{0:0}/{1:0}", targetHealth.GetCurrentValue(), targetHealth.GetMaxValue());
            enemyClassText.text = targetHealth.GetComponent<BaseStats>().GetCharacterClass().ToString() + ": ";
        }
    }   
}
