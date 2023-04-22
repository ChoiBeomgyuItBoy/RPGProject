using GameDevTV.Inventories;
using GameDevTV.UI.Inventories;
using UnityEngine;

namespace RPG.UI.Shops
{
    public class ItemHolder : MonoBehaviour, IItemHolder
    {
        InventoryItem item;

        public void Setup(InventoryItem item)
        {
            this.item = item;
        }

        public InventoryItem GetItem()
        {
            return item;
        }
    }
}
