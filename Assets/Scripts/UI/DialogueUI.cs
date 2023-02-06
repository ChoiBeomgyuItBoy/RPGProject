using System;
using RPG.Dialogue;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class DialogueUI : MonoBehaviour
    {
        PlayerConversant playerConversant;
        [SerializeField] TextMeshProUGUI AIText;
        [SerializeField] Button nextButton;
        [SerializeField] Button quitButton;
        [SerializeField] GameObject AIResponse;
        [SerializeField] Transform choiceRoot;
        [SerializeField] GameObject choicePrefab;
        [SerializeField] TextMeshProUGUI conversantName;

        void Start()
        {
            playerConversant = GameObject.FindWithTag("Player").GetComponent<PlayerConversant>();

            playerConversant.onConversationUpdated += UpdateUI;

            nextButton.onClick.AddListener(() => playerConversant.Next());
            quitButton.onClick.AddListener(() => playerConversant.Quit());

            UpdateUI();
        }

        void UpdateUI()
        {
            gameObject.SetActive(playerConversant.IsActive());

            if(!playerConversant.IsActive())
            {
                return;
            }

            conversantName.text = playerConversant.GetCurrentConversantName();

            AIResponse.SetActive(!playerConversant.IsChoosing());
            choiceRoot.gameObject.SetActive(playerConversant.IsChoosing());

            if(playerConversant.IsChoosing())
            {
                BuildChoiceList();
            }
            else
            {
                AIText.text = playerConversant.GetText();
                nextButton.gameObject.SetActive(playerConversant.HasNext());
            }
        }

        void BuildChoiceList()
        {
            foreach (Transform item in choiceRoot)
            {
                Destroy(item.gameObject);
            }

            foreach (DialogueNode choice in playerConversant.GetChoices())
            {
                GameObject choiceInstance = Instantiate(choicePrefab, choiceRoot);
                var textComponent = choiceInstance.GetComponentInChildren<TextMeshProUGUI>();
                textComponent.text = choice.GetText();

                Button button = choiceInstance.GetComponentInChildren<Button>();
                button.onClick.AddListener(() => 
                {
                    playerConversant.SelectChoice(choice);
                });
            }
        }
    }
}