public class PlayerInput : Singleton<PlayerInput>
{
    private PlayerOperation _playerOperationInput;

    void Awake()
    {
        _playerOperationInput = new PlayerOperation();
    }

    public PlayerOperation Input => _playerOperationInput;
}
