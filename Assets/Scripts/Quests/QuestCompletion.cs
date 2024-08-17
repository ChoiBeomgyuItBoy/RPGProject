using UnityEngine;

namespace RPG.Quests
{
    public class QuestCompletion : MonoBehaviour
    {
        [SerializeField] Quest quest;
        QuestList questList;

        // Unity Event Call
        public void CompleteObjective(string objectiveID)
        {
            questList.CompleteObjective(quest, objectiveID);
        }

        void Awake()
        {
            questList = GameObject.FindWithTag("Player").GetComponent<QuestList>();
        }
    }
}