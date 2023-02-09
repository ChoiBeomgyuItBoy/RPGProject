using RPG.Inventories;
using TMPro;
using UnityEngine;

namespace RPG.UI
{
    public class PurseUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI balanceField;

        Purse playerPurse = null;

        void Start()
        {
            playerPurse = GameObject.FindWithTag("Player").GetComponent<Purse>();
            playerPurse.onChange += RefreshUI;

            RefreshUI();
        }

        void RefreshUI()
        {
            balanceField.text = $"${playerPurse.GetBalance():N2}";
        }
    }
}