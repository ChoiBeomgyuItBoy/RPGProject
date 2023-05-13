using GameDevTV.Inventories;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Quests
{
    public class QuestRewardUI : MonoBehaviour
    {
        [SerializeField] Image itemIcon;
        [SerializeField] TMP_Text numberText;

        public void Setup(InventoryItem item, int number)
        {
            itemIcon.sprite = item.GetIcon();
            numberText.text = number.ToString();
        }
    }
}
