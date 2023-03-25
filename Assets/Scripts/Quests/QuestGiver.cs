using UnityEngine;

namespace RPG.Quests
{
    public class QuestGiver : MonoBehaviour
    {
        [SerializeField] Quest[] quests;

        // Called in Unity Event for dialogue action
        public void GiveQuest(int index)
        {
            QuestList questList = GameObject.FindWithTag("Player").GetComponent<QuestList>();
            questList.AddQuest(quests[index]);
        }
    }
}
