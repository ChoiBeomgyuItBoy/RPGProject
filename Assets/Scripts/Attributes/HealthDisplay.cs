using UnityEngine;
using TMPro;
using System;

namespace RPG.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        Health health;

        TMP_Text healthValueText;

        void Awake()
        {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
            healthValueText = GetComponent<TMP_Text>();
        }

        void Update()
        {
            healthValueText.text = string.Format("{0:0}/{1:0}", health.GetCurrentHealth(), health.GetMaxHealth());
        }
    }
}
