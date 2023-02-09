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
        [SerializeField] Button buyButton;
        [SerializeField] RowUI rowPrefab;
        [SerializeField] TextMeshProUGUI totalField;
        Shopper shopper = null;
        Shop currentShop = null;

        void Start()
        {
            shopper = GameObject.FindWithTag("Player").GetComponent<Shopper>();

            if(shopper != null)
            {
                shopper.activeShopChanged += ShopChanged;
                ShopChanged();
            }

            quitButton.onClick.AddListener(Close);     
            buyButton.onClick.AddListener(ConfirmTransaction);
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