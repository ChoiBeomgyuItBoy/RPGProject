using RPG.Quests;
using TMPro;
using UnityEngine;

namespace RPG.UI.Quests
{
    public class QuestTooltipUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] TextMeshProUGUI description;
        [SerializeField] Transform objectiveContainer;
        [SerializeField] GameObject objectivePrefab;
        [SerializeField] GameObject objectiveIncompletePrefab;
        [SerializeField] Transform rewardsContainer;
        [SerializeField] QuestRewardUI rewardPrefab;
        QuestStatus status;

        public void Setup(QuestStatus status)
        {   
            this.status = status;
            Quest quest = status.GetQuest();
            title.text = quest.GetTitle();
            description.text = quest.GetDescription();
            FillObjectives();
            FillRewards();
        }

        private void OnEnable()
        {
            if(status == null) return;
            FillObjectives();
        }

        private void FillObjectives()
        {
            foreach(Transform child in objectiveContainer)
            {
                Destroy(child.gameObject);
            }

            foreach(var objective in status.GetQuest().GetObjectives())
            {
                GameObject prefab = objectiveIncompletePrefab;

                if(status.IsObjectiveComplete(objective.reference))
                {
                    prefab = objectivePrefab;
                }

                GameObject objectiveInstance = Instantiate(prefab, objectiveContainer);
                objectiveInstance.GetComponentInChildren<TextMeshProUGUI>().text = objective.description;
            }
        }

        private void FillRewards()
        {
            foreach(Transform child in rewardsContainer)
            {
                Destroy(child.gameObject);
            }

            foreach(var reward in status.GetQuest().GetRewards())
            {
                var rewardInstance = Instantiate(rewardPrefab, rewardsContainer);
                rewardInstance.Setup(reward.item, reward.number);
            }
        }
    }   
}