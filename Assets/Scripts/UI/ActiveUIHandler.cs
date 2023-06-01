using RPG.Control;
using UnityEngine;

namespace RPG.Core
{
    public class ActiveUIHandler : MonoBehaviour
    {
        [SerializeField] Animator uiAnimator;
        [SerializeField] string enterAnimationTrigger = "";
        [SerializeField] string exitAnimationTrigger = "";
        [SerializeField] bool toggleControls = false;
        [SerializeField] bool toggleInput = false;
        [SerializeField] bool cancelCurrentAction = false;
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

            if(cancelCurrentAction)
            {
                playerController.GetComponent<ActionScheduler>().CancelCurrentAction();
            }

            if(exitAnimationTrigger != "")
            {
                uiAnimator.ResetTrigger(exitAnimationTrigger);
            }

            if(enterAnimationTrigger != "")
            {
                uiAnimator.SetTrigger(enterAnimationTrigger);
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

            if(enterAnimationTrigger != "")
            {
                uiAnimator.ResetTrigger(enterAnimationTrigger);
            }

            if(exitAnimationTrigger != "")
            {
                uiAnimator.SetTrigger(exitAnimationTrigger);
            }
        }
    }
}
