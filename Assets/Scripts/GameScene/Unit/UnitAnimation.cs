using System.Collections.Generic;
using UnityEngine;

public class UnitAnimation : MonoBehaviour
{
    [Header("SpriteRenderer")]
    // �摜��\������
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [Header("�A�j���[�V�����p�摜")]
    [SerializeField] private Sprite[] _sprites;
    private int _spritePerDirection;

    [Header("�ړ�����Unit��UnitController")]
    // Unit�̈ړ����
    [SerializeField] private AbstractUnitController _unitController;


    [Header("�ړ�����UnitMove")]
    [SerializeField] private UnitMove _unitMove;

    private float _previousSpeed;

    // Sprite���ς�鎞��
    private float _spriteChangeTime;
    // �o��������
    private float _time = 0f;

    // ���݂�Sprite�̃C���f�b�N�X
    private int _currentSpriteIndex = 0;

    private static readonly Dictionary<eDirection, int> SpriteFirstIndexs = new Dictionary<eDirection, int>
    {
        { eDirection.Left, 6 },
        { eDirection.Right, 3 },
        { eDirection.Up, 9 },
        { eDirection.Down, 0 }
    };

    private eDirection _direction = eDirection.Down;

    /// <summary>
    /// �������������s���B
    /// �K�v�ȃR���|�[�l���g�̎擾��X�v���C�g���̌v�Z���s���B
    /// </summary>
    void Start()
    {
        _spriteRenderer ??= GetComponent<SpriteRenderer>();
        _unitController ??= GetComponent<AbstractUnitController>();
        _unitMove ??= GetComponent<UnitMove>();

#if DEBUG_MODE
        if (_spriteRenderer == null || _unitController == null || _unitMove == null)
        {
            Debug.LogError($"�K�v�ȃR���|�[�l���g���s�����Ă��܂�: " +
                           $"{(_spriteRenderer == null ? "SpriteRenderer " : "")}" +
                           $"{(_unitController == null ? "UnitController " : "")}" +
                           $"{(_unitMove == null ? "UnitMove" : "")}");
        }

        if (_sprites == null || _sprites.Length == 0)
        {
            Debug.LogError("�A�j���[�V�����p�̃X�v���C�g���ݒ肳��Ă��܂���B");
        }
        else if (_sprites.Length % 4 != 0)
        {
            Debug.LogError("�X�v���C�g�̐���4�����ɋϓ��ɕ����ł��܂���B");
        }
#endif

        // 4�����Ȃ̂ŃX�v���C�g�̐���4�Ŋ���ƕ������Ƃ̃X�v���C�g���ɂȂ�
        _spritePerDirection = _sprites.Length / 4;

        _previousSpeed = _unitMove.Speed;

        UpdateSpriteChangeTime();
    }

    /// <summary>
    /// ���t���[���Ăяo����鏈���B
    /// ���j�b�g�̈ړ������⑬�x�ɉ����ăA�j���[�V�������X�V����B
    /// </summary>
    void Update()
    {
        eDirection direction = GetCurrentDirection();

        // ���͂Ȃ��̂Ƃ�
        if (direction == eDirection.None)
        {
            StopAnimation();
            return;
        }

        // �����������Ƃ�
        if (direction == _direction)
        {
            UpdateAnimationIndex(direction);
        }
        else
        {
            ChangeAnimationDirection(direction);
        }

        // �K�p
        UpdateAnimation(_currentSpriteIndex);
    }

    /// <summary>
    /// ���݂̈ړ��������擾����B
    /// </summary>
    /// <returns>���݂̈ړ�������\�� eDirection�B</returns>
    private eDirection GetCurrentDirection()
    {
        UnitMoveStatus moveStatus = _unitController.unitMoveStatus;

        eDirection direction = eDirection.None;
        if (moveStatus.Left)
        {
            direction = eDirection.Left;
        }
        if (moveStatus.Right)
        {
            direction = eDirection.Right;
        }
        if (moveStatus.Up)
        {
            direction = eDirection.Up;
        }
        if (moveStatus.Down)
        {
            direction = eDirection.Down;
        }

        return direction;
    }

    /// <summary>
    /// �A�j���[�V�����̃C���f�b�N�X���X�V����B
    /// </summary>
    /// <param name="direction">���݂̈ړ������B</param>
    private void UpdateAnimationIndex(eDirection direction)
    {
        if (!CanUpdateAnimation())
        {
            return;
        }

        if (_currentSpriteIndex < SpriteFirstIndexs[direction] + _spritePerDirection - 1)
        {
            _currentSpriteIndex++;
        }
        else
        {
            _currentSpriteIndex = SpriteFirstIndexs[direction];
        }
    }

    /// <summary>
    /// �A�j���[�V�����̕�����ύX����B
    /// </summary>
    /// <param name="direction">�V�����ړ������B</param>
    private void ChangeAnimationDirection(eDirection direction)
    {
        _currentSpriteIndex = SpriteFirstIndexs[direction];
        _direction = direction;
    }

    /// <summary>
    /// �A�j���[�V�������~����B
    /// </summary>
    private void StopAnimation()
    {
        // �����I�������~�܂��Ă����Ԃɂ���
        _spriteRenderer.sprite = _sprites[SpriteFirstIndexs[_direction]];

        // �����̓A�j���[�V�������X�V����
        _time = _spriteChangeTime;
    }

    /// <summary>
    /// �w�肳�ꂽ�C���f�b�N�X�̃X�v���C�g��K�p����B
    /// </summary>
    /// <param name="index">�X�v���C�g�̃C���f�b�N�X�B</param>
    private void UpdateAnimation(int index)
    {
        _spriteRenderer.sprite = _sprites[index];
    }

    /// <summary>
    /// �A�j���[�V�������X�V�ł��邩�𔻒肷��B
    /// </summary>
    /// <returns>�X�V�\�ȏꍇ�� true�A����ȊO�� false�B</returns>
    private bool CanUpdateAnimation()
    {
        UpdateSpriteChangeTime();

        if (_time < _spriteChangeTime)
        {
            _time += Time.deltaTime;
            return false;
        }
        _time = 0f;

        return true;
    }

    /// <summary>
    /// �X�v���C�g���؂�ւ�鎞�Ԃ��X�V����B
    /// </summary>
    private void UpdateSpriteChangeTime()
    {
        if (_unitMove.Speed != _previousSpeed)
        {
            _spriteChangeTime = _unitMove.Speed * 0.05f;
            _previousSpeed = _unitMove.Speed;
        }
    }
}
