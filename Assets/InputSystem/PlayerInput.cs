using R3;
using UnityEngine.InputSystem;

public class PlayerInput : Singleton<PlayerInput>
{
    private PlayerOperation _input;
    void Start()
    {
        _input = new PlayerOperation();
    }

    public PlayerOperation Input => _input;

    public Observable<InputAction.CallbackContext> OnPerformed(InputAction action)
    {
        return Observable.FromEvent<InputAction.CallbackContext>(
            h => action.performed += h,
            h => action.performed -= h
        );
    }
}
