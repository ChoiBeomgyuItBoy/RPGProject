using UnityEngine;

namespace RPG.Quests
{
    public class QuestGiver : MonoBehaviour
    {
        [SerializeField] Quest quest;

        // Called in Unity Event for dialogue action
        public void GiveQuest()
        {
            QuestList questList = GameObject.FindWithTag("Player").GetComponent<QuestList>();
            questList.AddQuest(quest);
        }
    }
}
