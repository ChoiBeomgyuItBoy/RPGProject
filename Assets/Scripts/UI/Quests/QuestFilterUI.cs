using System;
using RPG.Quests;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Quests
{
    public class QuestFilterUI : MonoBehaviour
    {
        [SerializeField] QuestType questType;
        QuestList questList;
        Button button;

        public void Setup(QuestList questList)
        {
            this.questList = questList;
        }

        public void RefreshUI()
        {
            button.interactable = questList.GetFilter() != questType;
        }

        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(SelectFilter);
        }

        private void SelectFilter()
        {
            questList.SelectFilter(questType);
        }
    }
}
