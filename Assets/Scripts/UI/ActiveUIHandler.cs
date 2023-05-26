using RPG.Audio;
using RPG.Control;
using UnityEngine;

namespace RPG.Core
{
    public class ActiveUIHandler : MonoBehaviour
    {
        [SerializeField] bool toggleControls = false;
        PlayerController playerController;

        void Awake()
        {
            playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        }
        
        void OnEnable()
        {
            if(toggleControls)
            {
                playerController.enabled = false;
            }
        }

        void OnDisable()
        {
            if(toggleControls)
            {
                playerController.enabled = true;
            }
        }
    }
}
