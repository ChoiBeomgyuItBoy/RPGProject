using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RPG.Control
{
    public class InputReader : MonoBehaviour, Controls.IPlayerControlsActions
    {
        public bool IsClicking { get; private set; }

        private Controls controls;

        private void Start()
        {
            controls = new Controls();

            controls.PlayerControls.SetCallbacks(this);
            controls.PlayerControls.Enable();
        }

        private void OnDestroy()
        {
            controls.PlayerControls.Disable();
        }

        public void OnClick(InputAction.CallbackContext context)
        {
            if(context.performed) IsClicking = true; 
            
            else if(context.canceled) IsClicking = false; 
        }
    }
}

