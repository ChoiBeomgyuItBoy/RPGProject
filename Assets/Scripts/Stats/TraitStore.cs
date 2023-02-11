using System.Collections.Generic;
using GameDevTV.Saving;
using UnityEngine;

namespace RPG.Stats
{
    public class TraitStore : MonoBehaviour, IModifierProvider, ISaveable
    {
        [SerializeField] TraitBonus[] bonusConfig;
        
        Dictionary<Trait, int> assignedPoints = new Dictionary<Trait, int>();
        Dictionary<Trait, int> stagedPoints = new Dictionary<Trait, int>();

        Dictionary<Stat, Dictionary<Trait, float>> additiveBonusCache;
        Dictionary<Stat, Dictionary<Trait, float>> percentageBonusCache;


        [System.Serializable]
        class TraitBonus
        {
            public Trait trait;
            public Stat stat;
            public float additiveBonusPerPoint = 0;
            public float percentageBonusPerPoint = 0;
        }

        private void Awake()
        {
            additiveBonusCache = new Dictionary<Stat, Dictionary<Trait, float>>();
            percentageBonusCache = new Dictionary<Stat, Dictionary<Trait, float>>();

            foreach(var bonus in bonusConfig)
            {
                if(!additiveBonusCache.ContainsKey(bonus.stat))
                {
                    additiveBonusCache[bonus.stat] = new Dictionary<Trait, float>();
                }

                if(!percentageBonusCache.ContainsKey(bonus.stat))
                {
                    percentageBonusCache[bonus.stat] = new Dictionary<Trait, float>();
                }

                additiveBonusCache[bonus.stat][bonus.trait] = bonus.additiveBonusPerPoint;
                percentageBonusCache[bonus.stat][bonus.trait] = bonus.percentageBonusPerPoint;
            }
        }

        public int GetUnassignedPoints()
        {
            return GetAssignablePoints() - GetTotalProposedPoints();
        }

        public int GetTotalProposedPoints()
        {
            int total = 0;

            foreach(int points in assignedPoints.Values)
            {
                total += points;
            }

            
            foreach(int points in stagedPoints.Values)
            {
                total += points;
            }

            return total;
        }

        public int GetProposedPoints(Trait trait)
        {
            return GetPoints(trait) + GetStagedPoints(trait);
        }

        public int GetPoints(Trait trait)
        {
            return assignedPoints.ContainsKey(trait) ? assignedPoints[trait] : 0;
        }

        public int GetStagedPoints(Trait trait)
        {
            return stagedPoints.ContainsKey(trait) ? stagedPoints[trait] : 0;
        }

        public void AssignPoints(Trait trait, int points)
        {
            if(!CanAsignPoints(trait, points)) return;

            stagedPoints[trait] = GetStagedPoints(trait) + points;
        }

        public bool CanAsignPoints(Trait trait, int points)
        {
            if(GetStagedPoints(trait) + points < 0) return false;
            if(GetUnassignedPoints() < points) return false;

            return true;
        }

        public void Commit()
        {
            foreach(Trait trait in stagedPoints.Keys)
            {
                assignedPoints[trait] = GetProposedPoints(trait);
            }

            stagedPoints.Clear();
        }

        public int GetAssignablePoints()
        {
            return (int) GetComponent<BaseStats>().GetStat(Stat.TotalTraitPoints);
        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if(!additiveBonusCache.ContainsKey(stat)) yield break;

            foreach(Trait trait in additiveBonusCache[stat].Keys)
            {
                float bonus = additiveBonusCache[stat][trait];

                yield return bonus * GetPoints(trait);
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if(!percentageBonusCache.ContainsKey(stat)) yield break;

            foreach(Trait trait in percentageBonusCache[stat].Keys)
            {
                float bonus = percentageBonusCache[stat][trait];

                yield return bonus * GetPoints(trait);
            }
        }

        object ISaveable.CaptureState()
        {
            return assignedPoints;
        }

        void ISaveable.RestoreState(object state)
        {
            assignedPoints = new Dictionary<Trait, int>((Dictionary<Trait, int>)state);
        }
    }
}
