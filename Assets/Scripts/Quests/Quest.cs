using System.Collections.Generic;
using GameDevTV.Inventories;
using UnityEngine;

namespace RPG.Quests
{
    [CreateAssetMenu(menuName = "RPG/New Quest")]
    public class Quest : ScriptableObject
    {
        [SerializeField] List<Objective> objectives = new List<Objective>();
        [SerializeField] List<Reward> rewards = new List<Reward>();


        [System.Serializable]
        public class Objective
        {
            public string reference;
            [TextArea] public string description;
        }

        [System.Serializable]
        public class Reward
        {         
            [Min(1)] public int number;
            public InventoryItem item;
        }

        public static Quest GetByName(string questName)
        {
            foreach(Quest quest in Resources.LoadAll<Quest>(""))
            {
                if(questName == quest.name)
                {
                    return quest;
                }
            }

            return null;
        }

        public string GetTitle()
        {
            return name;
        }

        public int GetObjectiveCount()
        {
            return objectives.Count;
        }

        public IEnumerable<Objective> GetObjectives()
        {
            return objectives;
        }

        public IEnumerable<Reward> GetRewards()
        {
            return rewards;
        }

        public bool HasObjective(string objectiveReference)
        {
            foreach(Objective objective in objectives)
            {
                if(objective.reference == objectiveReference)
                {
                    return true;
                }
            }

            return false;
        }
    }
}