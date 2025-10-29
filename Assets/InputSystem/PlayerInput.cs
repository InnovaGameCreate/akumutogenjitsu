public class PlayerInput : Singleton<PlayerInput>
{
    private PlayerOperation _input;
    void Start()
    {
        _input = new PlayerOperation();
    }

    public PlayerOperation Input => _input;
}
