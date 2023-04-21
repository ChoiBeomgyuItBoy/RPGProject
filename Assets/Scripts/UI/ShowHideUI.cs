using RPG.Core;
using UnityEngine;

namespace RPG.UI
{
    public class ShowHideUI : MonoBehaviour
    {
        [SerializeField] PlayerSettings playerSettings;
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
            if (Input.GetKeyDown(GetUIKey()))
            {
                Toggle();
            }
        }

        KeyCode GetUIKey()
        {
            switch(menuType)
            {
                case MenuType.Pause:
                    return playerSettings.GetPauseKey();
                case MenuType.Inventory:
                    return playerSettings.GetInventoryKey();
                case MenuType.Quests:
                    return playerSettings.GetQuestsKey();
                case MenuType.Traits:
                    return playerSettings.GetTraitsKey();
            }

            return default;
        }

        public void Toggle()
        {
            uiContainer.SetActive(!uiContainer.activeSelf);
        }
    }
}