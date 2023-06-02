using System.Collections;
using RPG.Dialogue;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RPG.UI
{
    public class DialogueUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        PlayerConversant playerConversant;
        [SerializeField] TextMeshProUGUI AIText;
        [SerializeField] Button nextButton;
        [SerializeField] GameObject AIResponse;
        [SerializeField] Transform choiceRoot;
        [SerializeField] GameObject choicePrefab;
        [SerializeField] TextMeshProUGUI conversantName;
        [SerializeField] float normalDisplaySpeed = 0.03f;
        [SerializeField] float fastDisplaySpeed = 0.06f;
        float letterSpeed = 0;
        Coroutine displayCoroutine = null;

        void Start()
        {
            playerConversant = GameObject.FindWithTag("Player").GetComponent<PlayerConversant>();

            playerConversant.onConversationUpdated += UpdateUI;

            nextButton.onClick.AddListener(() => playerConversant.Next());

            UpdateUI();

            letterSpeed = normalDisplaySpeed;
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
                DisplayResponse();
            }
        }

        void DisplayResponse()
        {
            if(displayCoroutine != null)
            {
                StopCoroutine(displayCoroutine);
            }

            AIText.text = "";

            displayCoroutine = StartCoroutine(AITextRoutine());
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            letterSpeed = fastDisplaySpeed;
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            letterSpeed = normalDisplaySpeed;
        }

        IEnumerator AITextRoutine()
        {
            nextButton.gameObject.SetActive(false);

            for (int i = 0; i < playerConversant.GetText().Length; i++)
            {
                AIText.text += playerConversant.GetText()[i];
                yield return new WaitForSeconds(letterSpeed);
            }

            nextButton.gameObject.SetActive(true);
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