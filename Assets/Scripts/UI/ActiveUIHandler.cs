using RPG.Control;
using UnityEngine;

namespace RPG.Core
{
    public class ActiveUIHandler : MonoBehaviour
    {
        [SerializeField] bool toggleControls = false;
        [SerializeField] bool toggleInput = false;
        PlayerController playerController;
        InputReader inputReader;

        void Awake()
        {
            playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            inputReader = InputReader.GetPlayerInputReader();
        }
        
        void OnEnable()
        {
            if(toggleControls)
            {
                playerController.enabled = false;
            }

            if(toggleInput)
            {
                inputReader.enabled = false;
            }
        }

        void OnDisable()
        {
            if(toggleControls)
            {
                playerController.enabled = true;
            }

            if(toggleInput)
            {
                inputReader.enabled = true;
            }
        }
    }
}
