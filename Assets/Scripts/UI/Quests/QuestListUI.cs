using System.Linq;
using RPG.Quests;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Quests
{
    public class QuestListUI : MonoBehaviour
    {
        [SerializeField] QuestTooltipUI questTooltip;
        [SerializeField] Transform listRoot;
        [SerializeField] Button allQuestsButton;
        [SerializeField] Button pendingQuestsButton;
        [SerializeField] Button completedQuestsButton;
        [SerializeField] QuestItemUI questPrefab;
        QuestList questList;

        void Start()
        {
            questList = GameObject.FindWithTag("Player").GetComponent<QuestList>();
            questList.onListUpdated += Redraw;
            allQuestsButton.onClick.AddListener(ToggleAllQuests);
            pendingQuestsButton.onClick.AddListener(TogglePendingQuests);
            completedQuestsButton.onClick.AddListener(ToggleCompletedQuests);
            Redraw();
            InitTooltip();
        }

        void InitTooltip()
        {
            if(questList.GetStatuses().Count() > 0)
            {
                questTooltip.Setup(questList.GetLastStatus());
            }
            else
            {
                questTooltip.gameObject.SetActive(false);
            }
        }

        void Redraw()
        {
            foreach (Transform child in listRoot)
            {
                Destroy(child.gameObject);
            }

            foreach (QuestStatus questStatus in questList.GetStatuses().Reverse())
            {
                if(!completedQuestsButton.IsInteractable() && !questStatus.IsComplete()) continue;
                if(!pendingQuestsButton.IsInteractable() && questStatus.IsComplete()) continue;

                QuestItemUI questInstance = Instantiate<QuestItemUI>(questPrefab, listRoot);
                questInstance.Setup(questStatus, questTooltip);
            }
        }

        void ToggleAllQuests()
        {
            allQuestsButton.interactable = false;
            pendingQuestsButton.interactable = true;
            completedQuestsButton.interactable = true;
            Redraw();
        }

        void TogglePendingQuests()
        {
            pendingQuestsButton.interactable = false;
            allQuestsButton.interactable = true;
            completedQuestsButton.interactable = true;
            Redraw();
        }

        void ToggleCompletedQuests()
        {
            completedQuestsButton.interactable = false;
            allQuestsButton.interactable = true;
            pendingQuestsButton.interactable = true;
            Redraw();
        }
    }
}