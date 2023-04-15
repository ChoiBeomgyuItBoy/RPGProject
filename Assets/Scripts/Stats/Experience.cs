using System;
using GameDevTV.Inventories;
using GameDevTV.Saving;
using RPG.Inventories;
using UnityEngine;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable, IItemStore
    {
        [SerializeField] float experiencePoints = 0f;

        public event Action onExperienceGained;

        public void GainExperience(float experience)
        {
            experiencePoints += experience;
            onExperienceGained?.Invoke();
        }

        public float GetPoints()
        {
            return experiencePoints;
        }

        public object CaptureState()
        {
            return experiencePoints;
        }

        public void RestoreState(object state)
        {
            experiencePoints = (float) state;
        }

#if UNITY_EDITOR
        private void Update()
        {
            if(Input.GetKey(KeyCode.E))
            {
                GainExperience(Time.deltaTime * 1000);
            }
        }
#endif

        int IItemStore.AddItems(InventoryItem item, int number)
        {
            if(item is ExperienceItem)
            {
                GainExperience(item.GetPrice() * number);
                return number;
            }

            return 0;
        }
    }
}
