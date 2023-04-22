using UnityEngine;
using TMPro;

namespace RPG.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        [SerializeField] float colorChangeTreshold = 40;
        Health health;
        TMP_Text healthValueText;
        Color defaultTextColor;

        void Awake()
        {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
            healthValueText = GetComponent<TMP_Text>();
            defaultTextColor = healthValueText.color;
        }

        void Update()
        {
            healthValueText.color = health.GetCurrentHealth() < colorChangeTreshold? Color.red : defaultTextColor;
            healthValueText.text = string.Format("{0:0}/{1:0}", health.GetCurrentHealth(), health.GetMaxHealth());
        }
    }
}
