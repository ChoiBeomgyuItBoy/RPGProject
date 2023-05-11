using GameDevTV.Inventories;
using UnityEngine;

namespace RPG.UI
{
    public class ItemListUI : MonoBehaviour
    {
        [SerializeField] GameObject title;
        [SerializeField] Transform listRoot;
        [SerializeField] ItemRowUI itemRowPrefab;
        Inventory playerInventory;

        void Awake()
        {
            playerInventory = Inventory.GetPlayerInventory();
        }

        void Start()
        {
            title.SetActive(false);

            foreach(Transform child in listRoot)
            {
                Destroy(child.gameObject);
            }

            playerInventory.onItemAdded += AddItem;
        }

        void AddItem(InventoryItem item, int number)
        {
            title.SetActive(true);
            var itemRowInstance = Instantiate(itemRowPrefab, listRoot);
            itemRowInstance.Setup(item, number);
            itemRowInstance.onRemoved += CheckIfEmpty;
        }

        void CheckIfEmpty()
        {
            title.SetActive(listRoot.childCount > 1);
        }
    }
}