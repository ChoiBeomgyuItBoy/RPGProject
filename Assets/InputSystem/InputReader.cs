using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, Controls.IPlayerControlsActions
{
    public event Action ClickEvent;

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
        if(!context.performed) { return; }

        ClickEvent?.Invoke();
    }
}
