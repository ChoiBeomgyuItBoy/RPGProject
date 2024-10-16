using UnityEngine;
using System.Collections.Generic;

namespace RPG.Stats
{
    [CreateAssetMenu(menuName = "RPG/Stats/New Progression")]
    public class Progression : ScriptableObject
    {
        [System.Serializable]
        class ProgessionCharacterClass
        {
            public CharacterClass characterClass;
            public ProgressionStat[] stat;
        }

        [System.Serializable]

        class ProgressionStat
        {
            public Stat stat;
            public int[] levels;
        }

        [SerializeField] private ProgessionCharacterClass[] characterClasses = null;

        Dictionary<CharacterClass, Dictionary<Stat, int[]>> lookupTable = null;
        
        public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {
            BuildLookup();

            if(!lookupTable[characterClass].ContainsKey(stat))
            {
                return 0;
            }

            int[] levels = lookupTable[characterClass][stat];

            if(levels.Length == 0)
            {
                return 0;
            }

            if(level > levels.Length)
            {
                return levels[levels.Length - 1];
            }

            return levels[level - 1];
        }

        public int GetLevels(Stat stat, CharacterClass characterClass)
        {
            BuildLookup();

            if(!lookupTable[characterClass].ContainsKey(stat))
            {
                return 0;
            }
            
            int[] levels = lookupTable[characterClass][stat];

            return levels.Length;
        }

        private void BuildLookup()
        {
            if(lookupTable != null) return;

            lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, int[]>>();

            foreach(ProgessionCharacterClass progressionClass in characterClasses)
            {
                var statLookupTable = new Dictionary<Stat, int[]>();

                foreach(ProgressionStat progressionStat in progressionClass.stat)
                {
                    statLookupTable[progressionStat.stat] = progressionStat.levels;
                }

                lookupTable[progressionClass.characterClass] = statLookupTable;
            }
        }
    }
}

