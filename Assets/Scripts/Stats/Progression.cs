using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progession", menuName = "Stats/New Progression", order = 0)]
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
            public int[] level;
        }

        [SerializeField] private ProgessionCharacterClass[] progressionCharacterClasses = null;

        public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {
            foreach(ProgessionCharacterClass progressionClass in progressionCharacterClasses)
            {
                if(progressionClass.characterClass != characterClass) continue;
                
                foreach(ProgressionStat progressionStat in progressionClass.stat)
                {
                    if(progressionStat.stat != stat) continue;

                    if(progressionStat.level.Length < level) continue;
                    
                    return progressionStat.level[level - 1];
                }
            }

            return 0f;
        }

    
    }
}

