using UnityEngine;
using R3;

public class StartButtonModel : MonoBehaviour
{
    private readonly ReactiveProperty<int> _selectedIndex = new(0);
    public ReadOnlyReactiveProperty<int> SelectedIndex => _selectedIndex;

    private readonly int _numSelection = 3;

    public void SetSelectedIndex(int index)
    {
        if (index < 0 || index >= _numSelection) return;
        _selectedIndex.Value = index;
    }

    public void MoveRight()
    {
        if (_selectedIndex.Value >= _numSelection - 1) return;
        _selectedIndex.Value++;
    }

    public void MoveLeft()
    {
        if (_selectedIndex.Value <= 0) return;
        _selectedIndex.Value--;
    }
}
