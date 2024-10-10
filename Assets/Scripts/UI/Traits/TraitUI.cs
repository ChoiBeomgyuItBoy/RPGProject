using RPG.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Traits
{
    public class TraitUI : MonoBehaviour
    {
        [SerializeField] Button commitButton;
        [SerializeField] TextMeshProUGUI unassignedPointsText;

        TraitStore playerTraitStore = null;

        private void Start()
        {
            playerTraitStore = GameObject.FindWithTag("Player").GetComponent<TraitStore>();

            playerTraitStore.storeUpdated += UpdateUI;

            commitButton.onClick.AddListener(playerTraitStore.Commit);

            UpdateUI();
        }

        private void UpdateUI()
        {
            commitButton.interactable = playerTraitStore.GetTotalStagedPoints() > 0;
            unassignedPointsText.text = $"{playerTraitStore.GetUnassignedPoints()}";
        }
    }
}
