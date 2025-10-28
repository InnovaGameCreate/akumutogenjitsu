using System.Collections.Generic;
using R3;
using UnityEngine;

public class PasswordEventModel
{
    // ユーザが入力している数字
    private readonly ReactiveProperty<List<int>> _slotNums = new(new List<int>());
    public ReadOnlyReactiveProperty<List<int>> SlotNums => _slotNums;

    // パスワード
    private readonly ReactiveProperty<string> _correctPassword = new("");
    public ReadOnlyReactiveProperty<string> CorrectPassword => _correctPassword;

    // 選択中の桁
    private readonly ReactiveProperty<int> _activeSlotIndex = new(0);
    public ReadOnlyReactiveProperty<int> ActiveSlotIndex => _activeSlotIndex;

    public PasswordEventModel(string password, int defaultActiveSlot)
    {
        for (int i = 0; i < password.Length; i++)
        {
            _slotNums.Value.Add(0);
        }
        _correctPassword.Value = password;
        _activeSlotIndex.Value = defaultActiveSlot;
    }

    public void SetInputNum(int index, int num)
    {
        if (index < 0 || index >= _slotNums.Value.Count)
        {
            Debug.LogError($"不正な桁の入力がされました。index: {index}");
            return;
        }

        _slotNums.Value[index] = num;
    }

    /// <summary>
    /// 選択中の桁を左に動かす
    /// 左端に行ったら右端に移動させる
    /// </summary>
    public void MoveLeftSelection()
    {
        if (_activeSlotIndex.Value <= 0)
        {
            _activeSlotIndex.Value = _slotNums.Value.Count - 1;
            return;
        }
        _activeSlotIndex.Value--;
    }

    /// <summary>
    /// 選択中の桁を右に動かす
    /// 右端に行ったら左端に移動させる
    /// </summary>
    public void MoveRightSelection()
    {
        if (_activeSlotIndex.Value >= _slotNums.Value.Count - 1)
        {
            _activeSlotIndex.Value = 0;
            return;
        }

        _activeSlotIndex.Value++;
    }

    /// <summary>
    /// 指定した桁の値を増やす
    /// </summary>
    public void PlusNumber()
    {
        List<int> nums = new List<int>(_slotNums.Value);
        if (_activeSlotIndex.Value < 0 || _activeSlotIndex.Value >= _slotNums.Value.Count)
        {
            Debug.LogError($"不正な桁の入力がされました。index: {_activeSlotIndex.Value}");
            return;
        }
        if (nums[_activeSlotIndex.Value] >= 9)
        {
            nums[_activeSlotIndex.Value] = 0;
        }
        else
        {
            nums[_activeSlotIndex.Value]++;
        }

        _slotNums.Value = nums;
    }

    /// <summary>
    /// 指定した桁の値を減らす
    /// </summary>
    public void MinusNumber()
    {
        List<int> nums = new List<int>(_slotNums.Value);
        if (_activeSlotIndex.Value < 0 || _activeSlotIndex.Value >= _slotNums.Value.Count)
        {
            Debug.LogError($"不正な桁の入力がされました。index: {_activeSlotIndex.Value}");
            return;
        }

        if (nums[_activeSlotIndex.Value] <= 0)
        {
            nums[_activeSlotIndex.Value] = 9;
        }
        else
        {
            nums[_activeSlotIndex.Value]--;
        }

        _slotNums.Value = nums;
    }
}
