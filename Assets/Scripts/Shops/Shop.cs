using System;
using System.Collections.Generic;
using GameDevTV.Inventories;
using RPG.Control;
using RPG.Inventories;
using UnityEngine;

namespace RPG.Shops
{
    public class Shop : MonoBehaviour, IRaycastable
    {
        [SerializeField] string shopName = "";

        [SerializeField] StockItemConfig[] stockConfig;

        Dictionary<InventoryItem, int> transaction = new Dictionary<InventoryItem, int>();
        Dictionary<InventoryItem, int> stock = new Dictionary<InventoryItem, int>();

        Shopper currentShopper = null;

        public event Action onChange;


        [System.Serializable]
        class StockItemConfig
        {
            public InventoryItem item;
            public int initialStock;
            [Range(0, 100)] public float buyingDiscountPercentage;
        }

        private void Awake()
        {
            foreach(StockItemConfig config in stockConfig)
            {
                stock[config.item] = config.initialStock;
            }
        }

        public void SetShopper(Shopper shopper)
        {
            currentShopper = shopper;
        }

        public string GetShopName()
        {
            return shopName;
        }

        public IEnumerable<ShopItem> GetAllItems()
        {
            foreach(StockItemConfig config in stockConfig)
            {
                float price  = config.item.GetPrice() * (1 - config.buyingDiscountPercentage / 100);

                int quantityInTransaction = 0;

                transaction.TryGetValue(config.item, out quantityInTransaction);

                int currentStock = stock[config.item];

                yield return new ShopItem(config.item, currentStock, price, quantityInTransaction);
            }
        }

        public IEnumerable<ShopItem> GetFilteredItems() 
        {
            return GetAllItems();
        }

        public void SelectFilter(ItemCategory category) { }
        public ItemCategory GetFilter() { return ItemCategory.None; }
        public void SelectMode(bool isBuying) { }
        public bool IsBuyingMode() { return true; }

        public bool CanTransact() 
        { 
            if(IsTransactionEmpy()) return false;
            if(!HasSufficientFunds()) return false;
            if(!HasInventorySpace()) return false;

            return true;
        }

        public bool HasSufficientFunds()
        {
            Purse shopperPurse = currentShopper.GetComponent<Purse>();

            if(shopperPurse == null) return false;

            return shopperPurse.GetBalance() >= TransactionTotal();
        }

        public bool HasInventorySpace()
        {
            List<InventoryItem> flatItems = new List<InventoryItem>();
            Inventory shopperInventory = currentShopper.GetComponent<Inventory>();

            if(shopperInventory == null) return false;

            foreach(ShopItem shopItem in GetAllItems())
            {
                InventoryItem item = shopItem.GetInventoryItem();
                int quantity = shopItem.GetQuantityInTransaction();

                for (int i = 0; i < quantity; i++)
                {
                    flatItems.Add(item);
                }
            }

            return shopperInventory.HasSpaceFor(flatItems);
        }

        public bool IsTransactionEmpy()
        {
            return transaction.Count == 0;
        }

        public float TransactionTotal() 
        { 
            float total = 0;

            foreach(ShopItem item in GetAllItems())
            {
                total += item.GetPrice() * item.GetQuantityInTransaction();
            }

            return total;
        }

        public void AddToTransaction(InventoryItem item, int quantity) 
        { 
            if(!transaction.ContainsKey(item))
            {
                transaction[item] = 0;
            }

            if(transaction[item] + quantity > stock[item])
            {
                transaction[item] = stock[item];
            }
            else
            {
                transaction[item] += quantity;
            }

            if(transaction[item] <= 0)
            {
                transaction.Remove(item);
            }

            onChange?.Invoke();
        }

        public void ConfirmTransaction() 
        {
            Inventory shopperInventory = currentShopper.GetComponent<Inventory>();
            Purse shopperPurse = currentShopper.GetComponent<Purse>();

            if(shopperInventory == null || shopperPurse == null) return;

            foreach(ShopItem shopItem in GetAllItems())
            {
                InventoryItem item = shopItem.GetInventoryItem();
                int quantity = shopItem.GetQuantityInTransaction();
                float price = shopItem.GetPrice();

                for (int i = 0; i < quantity; i++)
                {
                    if(shopperPurse.GetBalance() < price) break;

                    bool success = shopperInventory.AddToFirstEmptySlot(item, 1);

                    if(success)
                    {
                        AddToTransaction(item, -1);
                        stock[item]--;
                        shopperPurse.UpdateBalance(-price);
                    }
                }
            }

            onChange?.Invoke();
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if(Input.GetMouseButtonDown(0))
            {
                callingController.GetComponent<Shopper>().SetActiveShop(this);
            }

            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Shop;
        }
    }
}