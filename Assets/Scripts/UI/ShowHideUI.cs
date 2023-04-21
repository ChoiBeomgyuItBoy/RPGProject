using RPG.Control;
using UnityEngine;

namespace RPG.UI
{
    public class ShowHideUI : MonoBehaviour
    {
        [SerializeField] InputReader inputReader;
        [SerializeField] MenuType menuType = default;
        [SerializeField] GameObject uiContainer = null;

        enum MenuType
        {
            Pause,
            Inventory,
            Quests,
            Traits
        }

        void Start()
        {
            uiContainer.SetActive(false);
        }

        void Update()
        {
            if (Input.GetKeyDown(GetKey()))
            {
                Toggle();
            }
        }

        KeyCode GetKey()
        {
            switch(menuType)
            {
                case MenuType.Pause:
                    return inputReader.GetPauseKey();
                case MenuType.Inventory:
                    return inputReader.GetInventoryKey();
                case MenuType.Quests:
                    return inputReader.GetQuestsKey();
                case MenuType.Traits:
                    return inputReader.GetTraitsKey();
            }

            return default;
        }

        public void Toggle()
        {
            uiContainer.SetActive(!uiContainer.activeSelf);
        }
    }
}