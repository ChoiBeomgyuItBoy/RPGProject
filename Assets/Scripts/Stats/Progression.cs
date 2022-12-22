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
            public float[] healthAtLevel;
        }

        [SerializeField] private ProgessionCharacterClass[] progressionCharacterClasses = null;

        internal float GetHealth(CharacterClass characterClass, int level)
        {
            foreach(ProgessionCharacterClass progressionClass in progressionCharacterClasses)
            {
                if(progressionClass.characterClass == characterClass)
                {
                    return progressionClass.healthAtLevel[level - 1];
                }
            }

            return 0f;
        }
    }
}

