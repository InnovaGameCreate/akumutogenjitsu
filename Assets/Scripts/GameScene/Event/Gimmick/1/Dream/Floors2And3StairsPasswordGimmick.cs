using UnityEngine;

public class Floors2And3StairsPasswordGimmick : AbstractEvent
{
    [Header("階段の壁")]
    [SerializeField] private GameObject _wall;

    [Header("パスワード")]
    [SerializeField] private string _password;

    // Playerが入力したパスワード
    private string _userInput;

    private bool _isPlayerIn = false;
    private bool _hasFinished = false;

    void Start()
    {
        if (_wall == null)
        {
            Debug.LogError("壁オブジェクトが設定されていません。");
        }
    }

    public override bool IsTriggerEvent()
    {
        return _isPlayerIn && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Z));
    }

    public override void TriggerEvent()
    {
        // TODO: UIからパスワード入力を受け取る処理に置き換える
        _userInput = "password";

        if (_userInput == _password)
        {
            if (_wall != null)
            {
                _wall.SetActive(false);
            }
            Debug.Log("ようやくこれで１階に...");
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("パスワードが間違っています");
        }

        _hasFinished = true;
    }

    public override bool IsFinishEvent()
    {
        return _hasFinished;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _isPlayerIn = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _isPlayerIn = false;
        }
    }
}

