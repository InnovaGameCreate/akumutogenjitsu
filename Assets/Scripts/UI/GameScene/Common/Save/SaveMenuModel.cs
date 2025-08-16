using R3;
using UnityEngine;

public class SaveMenuModel : MonoBehaviour
{
    public ReadOnlyReactiveProperty<int> ActiveSlotIndex => _activeSlotIndex;
    private readonly ReactiveProperty<int> _activeSlotIndex = new(0);

    private const int _maxSlot = 3;

    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        _activeSlotIndex.Value = 0;
    }

    public void MoveUpSlot()
    {
        _activeSlotIndex.Value = (_activeSlotIndex.Value - 1 + _maxSlot) % _maxSlot;
    }

    public void MoveDownSlot()
    {
        _activeSlotIndex.Value = (_activeSlotIndex.Value + 1) % _maxSlot;
    }
}