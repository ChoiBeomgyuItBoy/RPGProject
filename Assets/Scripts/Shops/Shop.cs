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
        [SerializeField] [Range(0,100)] float sellingDiscountPercentage = 80f;

        Dictionary<InventoryItem, int> transaction = new Dictionary<InventoryItem, int>();
        Dictionary<InventoryItem, int> stock = new Dictionary<InventoryItem, int>();

        Shopper currentShopper = null;
        bool isBuyingMode = true;
        ItemCategory filter = ItemCategory.None;

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
                float price = GetPrice(config);

                int quantityInTransaction = 0;

                transaction.TryGetValue(config.item, out quantityInTransaction);

                int availability = GetAvailability(config.item);

                yield return new ShopItem(config.item, availability, price, quantityInTransaction);
            }
        }

        public IEnumerable<ShopItem> GetFilteredItems() 
        {
            foreach(ShopItem shopItem in GetAllItems())
            {
                InventoryItem item = shopItem.GetInventoryItem();

                if(filter == ItemCategory.None || item.GetItemCategory() == filter)
                {
                    yield return shopItem;
                }
            }
        }

        public void SelectFilter(ItemCategory category) 
        { 
            filter = category;
            onChange?.Invoke();
        }

        public ItemCategory GetFilter() 
        { 
            return filter; 
        }

        public void SelectMode(bool isBuying) 
        { 
            isBuyingMode = isBuying;
            onChange?.Invoke();
        }

        public bool IsBuyingMode() 
        { 
            return isBuyingMode;     
        }

        public bool HasSufficientFunds()
        {
            if(!IsBuyingMode()) return true;

            Purse shopperPurse = currentShopper.GetComponent<Purse>();

            if(shopperPurse == null) return false;

            return shopperPurse.GetBalance() >= TransactionTotal();
        }

        public bool HasInventorySpace()
        {
            if(!IsBuyingMode()) return true;
            
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

        public bool CanTransact() 
        { 
            if(IsTransactionEmpy()) return false;
            if(!HasSufficientFunds()) return false;
            if(!HasInventorySpace()) return false;

            return true;
        }

        public void AddToTransaction(InventoryItem item, int quantity) 
        { 
            if(!transaction.ContainsKey(item))
            {
                transaction[item] = 0;
            }

            int availability = GetAvailability(item);

            if(transaction[item] + quantity > availability)
            {
                transaction[item] = availability;
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
                    if(IsBuyingMode())
                    {
                        BuyItem(shopperInventory, shopperPurse, item, price);
                    }
                    else
                    {
                        SellItem(shopperInventory, shopperPurse, item, price);
                    }

                }
            }

            onChange?.Invoke();
        }

        private void BuyItem(Inventory shopperInventory, Purse shopperPurse, InventoryItem item, float price)
        {
            if (shopperPurse.GetBalance() < price) return;

            bool success = shopperInventory.AddToFirstEmptySlot(item, 1);

            if (success)
            {
                AddToTransaction(item, -1);
                stock[item]--;
                shopperPurse.UpdateBalance(-price);
            }
        }

        private void SellItem(Inventory shopperInventory, Purse shopperPurse, InventoryItem item, float price)
        {
            int slot = FindFirstItemSlot(shopperInventory, item);

            if(slot == -1) return;

            AddToTransaction(item, -1);
            shopperInventory.RemoveFromSlot(slot, 1);
            stock[item]++;
            shopperPurse.UpdateBalance(price);
        }
        private int GetAvailability(InventoryItem item)
        {
            if(IsBuyingMode())
            {
                return stock[item];
            }
            
            return CountItemsInInventory(item);
        }

        private int CountItemsInInventory(InventoryItem item)
        {
            Inventory shopperInventory = currentShopper.GetComponent<Inventory>();

            if(shopperInventory == null) return 0;

            int total = 0;

            for (int i = 0; i < shopperInventory.GetSize(); i++)
            {
                if(shopperInventory.GetItemInSlot(i) == item)
                {
                    total += shopperInventory.GetNumberInSlot(i);
                }
            }

            return total;
        }

        private int FindFirstItemSlot(Inventory shopperInventory, InventoryItem item)
        {
            for (int i = 0; i < shopperInventory.GetSize(); i++)
            {
                if(shopperInventory.GetItemInSlot(i) == item)
                {
                    return i;
                }
            }

            return -1;
        }

        private float GetPrice(StockItemConfig config)
        {
            if(IsBuyingMode())
            {
                return config.item.GetPrice() * (1 - config.buyingDiscountPercentage / 100);
            }

            return config.item.GetPrice() * (sellingDiscountPercentage / 100);
        }

        bool IRaycastable.HandleRaycast(PlayerController callingController)
        {
            if(Input.GetMouseButtonDown(0))
            {
                callingController.GetComponent<Shopper>().SetActiveShop(this);
            }

            return true;
        }

        CursorType IRaycastable.GetCursorType()
        {
            return CursorType.Shop;
        }
    }
}