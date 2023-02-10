using System;
using GameDevTV.Saving;
using UnityEngine;

namespace RPG.Inventories
{
    public class Purse : MonoBehaviour, ISaveable
    {
        [SerializeField] float startingBalance = 400;

        float balance = 0;

        public event Action onChange;

        void Awake()
        {
            balance = startingBalance;
        }

        public float GetBalance()
        {
            return balance;
        }

        public void UpdateBalance(float amount)
        {
            balance += amount;
            onChange?.Invoke();
        }

        object ISaveable.CaptureState()
        {
            return balance;
        }

        void ISaveable.RestoreState(object state)
        {
            balance = (float) state;
        }
    }
}