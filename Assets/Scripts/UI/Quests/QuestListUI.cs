using RPG.Quests;
using UnityEngine;

namespace RPG.UI.Quests
{
    public class QuestListUI : MonoBehaviour
    {
        [SerializeField] QuestItemUI questPrefab;

        QuestList questList;

        void Start()
        {
            questList = GameObject.FindWithTag("Player").GetComponent<QuestList>();
            questList.onListUpdated += Redraw;
            Redraw();
        }

        private void Redraw()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            foreach (QuestStatus questStatus in questList.GetStatuses())
            {
                QuestItemUI questInstance = Instantiate<QuestItemUI>(questPrefab, transform);
                questInstance.Setup(questStatus);
            }
        }
    }
}