using R3;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プロローグデータモデル - 状態管理とデータ保持
/// </summary>
public class PrologueModel
{
    // 基本状態
    public ReactiveProperty<bool> IsFinished { get; private set; } = new ReactiveProperty<bool>(false);
    public ReactiveProperty<string> CurrentText { get; private set; } = new ReactiveProperty<string>("");
    public ReactiveProperty<float> FadeAlpha { get; private set; } = new ReactiveProperty<float>(0f);
    public ReactiveProperty<bool> IsTyping { get; private set; } = new ReactiveProperty<bool>(false);
    public ReactiveProperty<bool> WaitingForInput { get; private set; } = new ReactiveProperty<bool>(false);

    // 入力イベント用Subject
    private Subject<Unit> _inputSubject = new Subject<Unit>();
    public Observable<Unit> OnInputReceived => _inputSubject.AsObservable();

    // データ
    private List<PrologueEvent.PrologueLine> _prologueLines;
    private int _currentLineIndex = 0;

    public void Initialize(List<PrologueEvent.PrologueLine> prologueLines)
    {
        _prologueLines = prologueLines ?? new List<PrologueEvent.PrologueLine>();
        _currentLineIndex = 0;
        IsFinished.Value = false;
        CurrentText.Value = "";
        FadeAlpha.Value = 0f;
        IsTyping.Value = false;
        WaitingForInput.Value = false;
    }

    public PrologueEvent.PrologueLine GetCurrentLine()
    {
        if (HasValidCurrentLine())
        {
            return _prologueLines[_currentLineIndex];
        }
        return null;
    }

    public void MoveToNextLine()
    {
        _currentLineIndex++;
        if (_currentLineIndex >= _prologueLines.Count)
        {
            IsFinished.Value = true;
        }
    }

    public bool HasValidCurrentLine()
    {
        return _prologueLines != null &&
               _currentLineIndex >= 0 &&
               _currentLineIndex < _prologueLines.Count;
    }

    public void SetCurrentText(string text)
    {
        CurrentText.Value = text;
    }

    public void SetFadeAlpha(float alpha)
    {
        FadeAlpha.Value = Mathf.Clamp01(alpha);
    }

    public void SetIsTyping(bool isTyping)
    {
        IsTyping.Value = isTyping;
    }

    public void SetWaitingForInput(bool waiting)
    {
        WaitingForInput.Value = waiting;
    }

    public void TriggerInput()
    {
        _inputSubject.OnNext(Unit.Default);
    }

    public void Dispose()
    {
        IsFinished?.Dispose();
        CurrentText?.Dispose();
        FadeAlpha?.Dispose();
        IsTyping?.Dispose();
        WaitingForInput?.Dispose();
        _inputSubject?.Dispose();
    }
}
