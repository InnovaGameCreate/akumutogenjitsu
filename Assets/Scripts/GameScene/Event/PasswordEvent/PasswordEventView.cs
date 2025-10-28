using UnityEngine;
using R3;
using System.Collections.Generic;

public struct PasswordEventViewOutput
{
    public Observable<Unit> onMoveRight;
    public Observable<Unit> onMoveLeft;
    public Observable<Unit> onPlusNumber;
    public Observable<Unit> onMinusNumber;
}

public class PasswordEventView : MonoBehaviour
{
    [Header("スロット同士のマージン")]
    [SerializeField] private float _marginSlots = 100;

    [Header("スロットの中央")]
    [SerializeField] private Vector2 _centerPosition;

    [SerializeField] private PasswordEventSlotView _slotPrefab;
    [SerializeField] private GameObject _slotParent;

    // 入力
    private readonly Subject<Unit> _onMoveRight = new();
    private readonly Subject<Unit> _onMoveLeft = new();
    private readonly Subject<Unit> _onPlusNumber = new();
    private readonly Subject<Unit> _onMinusNumber = new();

    private List<GameObject> _slotViews = new List<GameObject>();

    public PasswordEventViewOutput Bind()
    {
        PasswordEventViewOutput output = new PasswordEventViewOutput();
        PlayerInput.Instance.Input.PasswordEvent.MoveLeft.performed += ctx =>
        {
            if (ctx.ReadValueAsButton())
            {
                _onMoveLeft.OnNext(Unit.Default);
            }
        };
        PlayerInput.Instance.Input.PasswordEvent.MoveRight.performed += ctx =>
        {
            if (ctx.ReadValueAsButton())
            {
                _onMoveRight.OnNext(Unit.Default);
            }
        };
        PlayerInput.Instance.Input.PasswordEvent.PlusNumber.performed += ctx =>
        {
            if (ctx.ReadValueAsButton())
            {
                _onPlusNumber.OnNext(Unit.Default);
            }
        };
        PlayerInput.Instance.Input.PasswordEvent.MinusNumber.performed += ctx =>
        {
            if (ctx.ReadValueAsButton())
            {
                _onMinusNumber.OnNext(Unit.Default);
            }
        };

        output.onMoveLeft = _onMoveLeft;
        output.onMoveRight = _onMoveRight;
        output.onPlusNumber = _onPlusNumber;
        output.onMinusNumber = _onMinusNumber;

        return output;
    }

    /// <summary>
    /// スロットを生成する
    /// </summary>
    /// <param name="slotNum"> スロットの数 </param>
    /// <param name="activeIndex"> デフォルトでアクティブにしておくindex </param>
    public void CreateSlots(int slotNum, int activeIndex)
    {
        if (_slotViews.Count != 0)
        {
            foreach (var obj in _slotViews)
            {
                Destroy(obj);
            }
            _slotViews.Clear();
        }

        float totalWidth = (slotNum - 1) * _marginSlots;

        float startX = _centerPosition.x - (totalWidth / 2f);

        for (int i = 0; i < slotNum; i++)
        {
            Vector3 position = new Vector3(
                startX + (i * _marginSlots),
                _centerPosition.y,
                0f
            );

            // Slot生成
            GameObject slot = Instantiate(_slotPrefab.gameObject, _slotParent.transform);
            slot.transform.localPosition = position;
            _slotViews.Add(slot);
        }

        SetActiveSlot(activeIndex);
    }

    /// <summary>
    /// スロットの数字を設定する
    /// </summary>
    /// <param name="index">　添字　</param>
    /// <param name="num"> 数字 </param>
    public void SetSlotNums(int index, int num)
    {
        if (_slotViews.Count == 0)
        {
            return;
        }
        PasswordEventSlotView view = _slotViews[index].GetComponent<PasswordEventSlotView>();
        view.SetNumber(num);
    }

    /// <summary>
    /// Activeなスロットを設定する
    /// </summary>
    /// <param name="index"> スロットのindex </param>
    public void SetActiveSlot(int index)
    {
        for (int i = 0; i < _slotViews.Count; i++)
        {
            _slotViews[i]?.GetComponent<PasswordEventSlotView>().SetActiveSlot(i == index);
        }
    }

    void OnDestroy()
    {
        _onMoveRight?.Dispose();
        _onMoveLeft?.Dispose();
        _onPlusNumber?.Dispose();
        _onMinusNumber?.Dispose();
    }
}
