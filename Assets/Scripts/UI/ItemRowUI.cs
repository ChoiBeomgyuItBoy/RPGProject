using System;
using System.Collections;
using GameDevTV.Inventories;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class ItemRowUI : MonoBehaviour
    {
        [SerializeField] TMP_Text itemNameText;
        [SerializeField] Image itemIcon;
        [SerializeField] float timeToRemove = 4;
        InventoryItem item;

        public event Action onRemoved; 
        
        public void Setup(InventoryItem item, int number)
        {
            if(item == null) return;

            this.item = item;
            itemNameText.text = $"{item.GetDisplayName()} x {number}";
            itemIcon.sprite = item.GetIcon();
        }

        void OnEnable()
        {
            StartCoroutine(RemoveRoutine());
        }

        IEnumerator RemoveRoutine()
        {
            yield return new WaitForSeconds(timeToRemove);
            onRemoved?.Invoke();
            Destroy(gameObject);
        }
    }
}