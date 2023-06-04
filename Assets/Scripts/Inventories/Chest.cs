using System;
using GameDevTV.Inventories;
using GameDevTV.Saving;
using RPG.Control;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Inventories
{
    [RequireComponent(typeof(RandomDropper))]
    public class Chest : MonoBehaviour, IRaycastable, ISaveable
    {
        [SerializeField] LootConfig[] loot;
        [SerializeField] UnityEvent onChestOpened;
        Inventory playerInventory;
        Boolean alreadyLooted = false;

        [System.Serializable]
        class LootConfig
        {
            public InventoryItem item;
            [Min(1)] public int number;
        }

        void Awake()
        {
            playerInventory = Inventory.GetPlayerInventory();
        }

        void LootChest()
        {
            alreadyLooted = true;
            
            foreach(var slot in loot)
            {
                GetComponent<RandomDropper>().DropItem(slot.item, slot.number);
            }
        }

        CursorType IRaycastable.GetCursorType()
        {
            return CursorType.Pickup;
        }

        bool IRaycastable.HandleRaycast(PlayerController callingController)
        {
            if(!alreadyLooted)
            {
                if(Input.GetKeyDown(callingController.GetInteractionKey()))
                {
                    onChestOpened?.Invoke();
                    GetComponent<Animation>().Play();
                    LootChest();
                }

                return true;
            }

            return false;
        }

        object ISaveable.CaptureState()
        {
            return alreadyLooted;
        }

        void ISaveable.RestoreState(object state)
        {
            alreadyLooted = (Boolean) state;

            if(alreadyLooted)
            {
                GetComponent<Animation>().Play();
            }
        }
    }
}
