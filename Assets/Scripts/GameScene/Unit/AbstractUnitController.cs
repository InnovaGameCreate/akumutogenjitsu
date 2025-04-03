using UnityEngine;

public abstract class AbstractUnitController : MonoBehaviour
{
    private UnitMove _unitMove;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _unitMove = GetComponent<UnitMove>();
        if (_unitMove == null)
        {
            Debug.LogError("UnitMove���A�^�b�`����Ă��܂���");
        }
    }

    private void Update()
    {
        Move();
    }

    /// <summary>
    /// UnitController�̏���������(Start()�̑���)
    /// </summary>
    protected virtual void OnStartUnitController()
    {
        return;
    }

    /// <summary>
    /// UnitController�̍X�V����(Update()�̑���)
    /// </summary>
    protected virtual void OnUpdateUnitController()
    {
        return;
    }

    /// <summary>
    /// Unit�̈ړ���Ԃ��擾����
    /// </summary>
    /// <returns> �ړ���� </returns>
    public abstract UnitMoveStatus GetMoveStatus();

    /// <summary>
    /// Unit�̈ړ�
    /// </summary>
    protected void Move()
    {
        _unitMove.Move(GetMoveStatus());
    }
}
