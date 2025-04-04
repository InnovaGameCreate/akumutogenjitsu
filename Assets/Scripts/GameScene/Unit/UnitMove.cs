using UnityEngine;

public class UnitMove : MonoBehaviour
{
    // ���x
    [SerializeField] private float _defaultSpeed;
    private float _speed;

    // ����
    private UnitMoveStatus _unitMoveStatus;
    
    // Unit���ړ����邩
    private bool _isEnabled;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_defaultSpeed == 0)
        {
            Debug.LogError("DefaultSpeed��0�ɐݒ肳��Ă��܂�");
        }
        _speed = _defaultSpeed;
        _isEnabled = true;
    }

    // Update is called once per frame
    void Update()
    {
    }

    /// <summary>
    /// Unit�𓮂���
    /// </summary>
    /// <param name="direction"> ���������� </param>
    public void Move(UnitMoveStatus unitmovestatus)
    {
        if (!_isEnabled)
        {
            return;
        }

        Vector2 move = Vector2.zero;

        if (unitmovestatus.Left)
        {
            move.x = -1;
        }
        if (unitmovestatus.Right)
        {
            move.x = 1;
        }
        if (unitmovestatus.Up)
        {
            move.y = 1;
        }
        if (unitmovestatus.Down)
        {
            move.y = -1;
        }
        move = move.normalized * _speed * Time.deltaTime;

        // �ړ�
        transform.position += (Vector3)move;

        // �������擾
        _unitMoveStatus = unitmovestatus;
    }

    /// <summary>
    /// Unit�̑��x
    /// </summary>
    public float Speed
    {
        get
        {
            return _speed;
        }

        set
        {
            if (value < 0)
            {
                Debug.LogError("Speed�ɕ��̒l���ݒ肳��Ă��܂�");
                return;
            }

            // ���x��ݒ�
            _speed = value;

            #if DEBUG_MODE
                Debug.Log("Speed��" + _speed + "�ɐݒ肵�܂���");
            #endif
        }
    }

    /// <summary>
    /// Unit���ړ����邩
    /// </summary>
    public bool IsEnabled
    {
        get
        {
            return _isEnabled;
        }
        set
        {
            _isEnabled = value;
            enabled = _isEnabled;
        }
    }
}
