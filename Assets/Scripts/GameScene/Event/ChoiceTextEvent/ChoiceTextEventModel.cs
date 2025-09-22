using R3;
using System.Collections.Generic;

public class ChoiceTextEventModel
{
    private readonly ReactiveProperty<List<Choice>> _choices = new();
    public ReadOnlyReactiveProperty<List<Choice>> Choices => _choices;

    private readonly ReactiveProperty<int> _selectedIndex = new();
    public ReadOnlyReactiveProperty<int> SelectedIndex => _selectedIndex;

    private readonly ReactiveProperty<string> _message = new();
    public ReadOnlyReactiveProperty<string> Message => _message;

    private readonly Subject<Choice> _choiceSelected = new();
    public Observable<Choice> OnChoiceSelected => _choiceSelected;

    public ChoiceTextEventModel(List<Choice> choices, string message)
    {
        SetChoices(choices);
        SetMessage(message);
    }

    /// <summary>
    /// 選択を下げる
    /// </summary>
    public void MoveToDown()
    {
        if (_selectedIndex.Value < _choices.Value.Count - 1)
        {
            _selectedIndex.Value++;
        }
    }

    /// <summary>
    /// 選択を上げる
    /// </summary>
    public void MoveToUp()
    {
        if (_selectedIndex.Value > 0)
        {
            _selectedIndex.Value--;
        }
    }

    /// <summary>
    /// 選択肢を追加する
    /// </summary>
    /// <param name="choice"> 選択肢 </param>
    public void AddChoice(Choice choice)
    {
        _choices.Value.Add(choice);
    }

    /// <summary>
    /// 全ての選択肢を設定する
    /// </summary>
    /// <param name="choices"> 全ての選択肢 </param>
    public void SetChoices(List<Choice> choices)
    {
        _choices.Value = choices;
    }

    /// <summary>
    /// メッセージを設定する
    /// </summary>
    /// <param name="message"> メッセージ </param>
    public void SetMessage(string message)
    {
        _message.Value = message;
    }

    public void ConfirmSelection()
    {
        _choiceSelected.OnNext(_choices.Value[_selectedIndex.Value]);
    }

    public bool IsFinishChoice()
    {
        return !_choices.Value[_selectedIndex.Value].IsReturn;
    }
}
