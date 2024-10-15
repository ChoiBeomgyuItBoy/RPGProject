using System;
using System.Collections;
using GameDevTV.Inventories;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Inventories
{
    public class ItemRowUI : MonoBehaviour
    {
        [SerializeField] TMP_Text itemNameText;
        [SerializeField] Image itemIcon;
        public event Action onRemoved; 

        // Called in animation event
        public void Remove()
        {
            onRemoved?.Invoke();
            Destroy(gameObject);
        }
        
        public void Setup(InventoryItem item, int number)
        {
            if(item == null) return;

            itemNameText.text = $"{item.GetDisplayName()} x {number}";
            itemIcon.sprite = item.GetIcon();
        }
    }
}