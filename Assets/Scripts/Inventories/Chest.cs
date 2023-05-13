using System;
using System.Collections;
using GameDevTV.Inventories;
using GameDevTV.Saving;
using RPG.Control;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Inventories
{
    public class Chest : MonoBehaviour, IRaycastable, ISaveable
    {
        [SerializeField] LootConfig[] loot;
        [SerializeField] float delayBetweenItems = 0.2f;
        [SerializeField] UnityEvent onChestOpen;
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

        IEnumerator LootChest()
        {
            alreadyLooted = true;
            
            foreach(var slot in loot)
            {
                playerInventory.AddToFirstEmptySlot(slot.item, slot.number);
                yield return new WaitForSeconds(delayBetweenItems);
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
                    onChestOpen?.Invoke();
                    GetComponent<Animation>().Play();
                    StartCoroutine(LootChest());
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
