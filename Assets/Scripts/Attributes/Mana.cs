using System;
using GameDevTV.Saving;
using GameDevTV.Utils;
using RPG.Stats;
using UnityEngine;

namespace RPG.Attributes
{
    public class Mana : MonoBehaviour, ISaveable, IStatsProvider
    {
        LazyValue<float> mana;

        public float GetCurrentValue()
        {
            return mana.value;
        }

        public float GetMaxValue()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Mana);
        }

        public bool UseMana(float manaToUse)
        {
            if(manaToUse > mana.value)
            {
                return false;
            }

            mana.value -= manaToUse;
            return true;
        }

        void Awake()
        {
            mana = new LazyValue<float>(GetMaxValue);
        }

        void Start()
        {
            mana.ForceInit();
        }

        void Update()
        {
            if(mana.value < GetMaxValue())
            {
                mana.value += Time.deltaTime * GetRegenRate();

                if(mana.value > GetMaxValue())
                {
                    mana.value = GetMaxValue();
                }
            }
        }

        float GetRegenRate()
        {
            return GetComponent<BaseStats>().GetStat(Stat.ManaRegenRate);
        }

        object ISaveable.CaptureState()
        {
            return mana.value;
        }

        void ISaveable.RestoreState(object state)
        {
            mana.value = (float) state;
        }
    }
}
