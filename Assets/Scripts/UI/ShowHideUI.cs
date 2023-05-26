using RPG.Control;
using UnityEngine;

namespace RPG.UI
{
    public class ShowHideUI : MonoBehaviour
    {
        [SerializeField] PlayerAction toggleAction = default;
        [SerializeField] GameObject uiContainer = null;
        InputReader inputReader;

        void Awake()
        {
            inputReader = InputReader.GetPlayerInputReader();
        }

        void Start()
        {
            uiContainer.SetActive(false);
        }

        void Update()
        {
            if (Input.GetKeyDown(inputReader.GetKeyCode(toggleAction)))
            {
                Toggle();
            }
        }

        public void Toggle()
        {
            uiContainer.SetActive(!uiContainer.activeSelf);
        }
    }
}