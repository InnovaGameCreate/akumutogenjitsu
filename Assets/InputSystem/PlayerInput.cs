public class PlayerInput : Singleton<PlayerInput>
{
    public PlayerOperation Input;
    void Start()
    {
        Input = new PlayerOperation();
    }
}
