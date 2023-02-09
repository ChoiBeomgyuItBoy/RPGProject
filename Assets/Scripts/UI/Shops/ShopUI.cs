using System;
using RPG.Shops;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Shops
{
    public class ShopUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI shopName;
        [SerializeField] Transform listRoot;
        [SerializeField] Button quitButton;
        [SerializeField] Button confirmButton;
        [SerializeField] RowUI rowPrefab;
        [SerializeField] TextMeshProUGUI totalField;
        Shopper shopper = null;
        Shop currentShop = null;

        Color originalTotalTextColor;

        void Start()
        {
            originalTotalTextColor = totalField.color;
            shopper = GameObject.FindWithTag("Player").GetComponent<Shopper>();

            if(shopper != null)
            {
                shopper.activeShopChanged += ShopChanged;
                ShopChanged();
            }

            confirmButton.onClick.AddListener(ConfirmTransaction);
            quitButton.onClick.AddListener(Close);     
        }

        void ShopChanged()
        {
            if(currentShop != null)
            {
                currentShop.onChange -= RefreshUI;
            }

            currentShop = shopper.GetActiveShop();

            gameObject.SetActive(currentShop != null);

            if(currentShop != null)   
            {
                shopName.text = currentShop.GetShopName();
                currentShop.onChange += RefreshUI;
                RefreshUI();
            }
        }

        void RefreshUI()
        {
            foreach(Transform child in listRoot)
            {
                Destroy(child.gameObject);
            }

            foreach(ShopItem item in currentShop.GetFilteredItems())
            {
                RowUI row = Instantiate<RowUI>(rowPrefab, listRoot);
                row.Setup(currentShop, item);
            }

            totalField.text = $"Total: ${currentShop.TransactionTotal():N2}";

            totalField.color = currentShop.HasSufficientFunds() ? originalTotalTextColor : Color.red;

            confirmButton.interactable = currentShop.CanTransact();
        }

        void Close()
        {
            shopper.SetActiveShop(null);
        }

        void ConfirmTransaction()
        {
            currentShop.ConfirmTransaction();
        }
    }
}