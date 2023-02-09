using System;
using UnityEngine;

namespace RPG.Shops
{
    public class Shopper : MonoBehaviour
    {
        Shop activeShop = null;

        public event Action activeShopChanged;

        public void SetActiveShop(Shop shop)
        {
            if(activeShop != null)
            {
                activeShop.SetShopper(null);
            }

            activeShop = shop;

            if(activeShop != null)
            {
                activeShop.SetShopper(this);
            }

            activeShopChanged?.Invoke();
        }

        public Shop GetActiveShop()
        {
            return activeShop;
        }
    }
}
