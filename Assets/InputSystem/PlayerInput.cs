using R3;
using UnityEngine.InputSystem;

public class PlayerInput : Singleton<PlayerInput>
{
    private PlayerActionInput _input;

    protected override void Awake()
    {
        base.Awake();
        _input = new PlayerActionInput();
    }
    
    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }

    private void OnDestroy()
    {
        _input?.Dispose();
    }

    public PlayerActionInput Input => _input;

    public Observable<InputAction.CallbackContext> OnPerformed(InputAction action)
    {
        return Observable.FromEvent<InputAction.CallbackContext>(
            h => action.performed += h,
            h => action.performed -= h
        );
    }
}
