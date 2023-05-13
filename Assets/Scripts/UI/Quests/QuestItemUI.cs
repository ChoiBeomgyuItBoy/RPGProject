using RPG.Quests;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Quests
{
    public class QuestItemUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] TextMeshProUGUI progress;
        QuestTooltipUI questTooltip;
        QuestStatus status;

        public void Setup(QuestStatus status, QuestTooltipUI questTooltip)
        {
            this.status = status;
            this.questTooltip = questTooltip;
            Color textColor = status.IsComplete() ? Color.gray : Color.white;
            title.color = textColor;
            progress.color = textColor;
            title.text = status.GetQuest().GetTitle();
            progress.text = status.GetCompletedCount() + "/" + status.GetQuest().GetObjectiveCount();
        }

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(SetTooltip);
        }

        private void SetTooltip()
        {
            if(!questTooltip.isActiveAndEnabled)
            {
                questTooltip.gameObject.SetActive(true);
            }

            questTooltip.Setup(status);
        }
    }
}