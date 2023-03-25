using System;
using System.Collections.Generic;
using GameDevTV.Inventories;
using GameDevTV.Saving;
using GameDevTV.Utils;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestList : MonoBehaviour, ISaveable, IPredicateEvaluator
    {
        //[SerializeField] Quest initialQuest;
        List<QuestStatus> statuses = new List<QuestStatus>();

        public event Action onListUpdated;

        // private void Start()
        // {
        //     AddQuest(initialQuest);
        // }

        public void AddQuest(Quest quest)
        {
            if(HasQuest(quest)) return;

            QuestStatus newStatus = new QuestStatus(quest);

            statuses.Add(newStatus);
            onListUpdated?.Invoke();
        }

        private bool HasQuest(Quest quest)
        {
            return GetQuestStatus(quest) != null;
        }

        public IEnumerable<QuestStatus> GetStatuses()
        {
            return statuses;
        }

        public void CompleteObjective(Quest quest, string objective)
        {
            QuestStatus status = GetQuestStatus(quest);

            if(status != null)
            {
                if(status.IsComplete())
                {
                    GiveReward(quest);
                }

                status.CompleteObjective(objective);

                onListUpdated?.Invoke();
            }
        }

        private QuestStatus GetQuestStatus(Quest quest)
        {
            foreach(QuestStatus status in statuses)
            {
                if(status.GetQuest() == quest)
                {
                    return status;
                }
            }

            return null;
        }

        private void GiveReward(Quest quest)
        {
            foreach(var reward in quest.GetRewards())
            {
                bool success = GetComponent<Inventory>().AddToFirstEmptySlot(reward.item, reward.number);

                if(!success)
                {
                    GetComponent<ItemDropper>().DropItem(reward.item, reward.number);
                }
            }
        }

        public object CaptureState()
        {
            List<object> state = new List<object>();

            foreach(QuestStatus status in statuses)
            {
                state.Add(status.CaptureState());
            }

            return state;
        }

        public void RestoreState(object state)
        {
            List<object> stateList = state as List<object>;

            if(stateList == null) return;

            statuses.Clear();

            foreach(object objectState in stateList)
            {
                statuses.Add(new QuestStatus(objectState));
            }

            onListUpdated?.Invoke();
        }

        public bool? Evaluate(string predicate, string[] parameters)
        {
            QuestStatus status = GetQuestStatus(Quest.GetByName(parameters[0]));

            switch(predicate)
            {
                case "HasQuest":

                    return HasQuest(Quest.GetByName(parameters[0]));

                case "CompletedQuest":

                    if(status != null)
                    {
                        return status.IsComplete();
                    }
                    return false;

                case "CompletedObjective":       

                    if(status != null)
                    {
                        return status.IsObjectiveComplete(parameters[1]);
                    } 
                    return false;    
            }

            return null;
        }
    }
}