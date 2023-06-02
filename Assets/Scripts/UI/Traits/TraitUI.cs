using RPG.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Traits
{
    public class TraitUI : MonoBehaviour
    {
        [SerializeField] Button commitButton;
        [SerializeField] Button quitButton;
        [SerializeField] TextMeshProUGUI unassignedPointsText;

        TraitStore playerTraitStore = null;

        private void Start()
        {
            playerTraitStore = GameObject.FindWithTag("Player").GetComponent<TraitStore>();

            playerTraitStore.storeUpdated += UpdateUI;

            commitButton.onClick.AddListener(playerTraitStore.Commit);
            quitButton.onClick.AddListener(() => playerTraitStore.SetTraitEnhancer(null));

            UpdateUI();
        }

        private void UpdateUI()
        {
            commitButton.interactable = playerTraitStore.GetTotalStagedPoints() > 0;
            gameObject.SetActive(playerTraitStore.IsActive());
            unassignedPointsText.text = $"{playerTraitStore.GetUnassignedPoints()}";
        }
    }
}
