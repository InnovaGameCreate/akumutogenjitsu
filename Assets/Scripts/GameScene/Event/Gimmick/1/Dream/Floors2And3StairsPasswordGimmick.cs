using UnityEngine;

public class Floors2And3StairsPasswordGimmick : AbstractEvent
{
    [Header("�K�i�̕�")]
    [SerializeField] private GameObject _wall;

    [Header("�p�X���[�h")]
    [SerializeField] private string _password;

    // Player�����͂����p�X���[�h
    private string _userInput;

    private bool _isPlayerIn = false;
    private bool _hasFinished = false;

    void Start()
    {
        if (_wall == null)
        {
            Debug.LogError("�ǃI�u�W�F�N�g���ݒ肳��Ă��܂���B");
        }
    }

    public override bool IsTriggerEvent()
    {
        return _isPlayerIn && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Z));
    }

    public override void TriggerEvent()
    {
        // TODO: UI����p�X���[�h���͂��󂯎�鏈���ɒu��������
        _userInput = "password";

        if (_userInput == _password)
        {
            if (_wall != null)
            {
                _wall.SetActive(false);
            }
            Debug.Log("�悤�₭����łP�K��...");
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("�p�X���[�h���Ԉ���Ă��܂�");
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

