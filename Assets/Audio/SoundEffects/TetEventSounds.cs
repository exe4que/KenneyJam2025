using KenneyJam2025;
using UnityEngine;
using UnityEngine.InputSystem;

public class TetEventSounds : MonoBehaviour

{
    public InputActionReference click;
    void Start()
    {
        click.action.performed += InvokeEvent;

    }

    // Update is called once per frame
    private void InvokeEvent(InputAction.CallbackContext obj)
    {
        GlobalEvents.PlayerDied?.Invoke();

    }
}
