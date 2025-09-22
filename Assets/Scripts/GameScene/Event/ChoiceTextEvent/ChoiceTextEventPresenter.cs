using System.Collections.Generic;
using UnityEngine;
using R3;

public class ChoiceTextEventPresenter
{
    private ChoiceTextEventView _view;
    private ChoiceTextEventModel _model;

    private CompositeDisposable _disposable = new();

    public ChoiceTextEventPresenter(ChoiceTextEventView view, List<Choice> choices, string message)
    {
        _view = view;
        _model = new ChoiceTextEventModel(choices, message);

        Bind();
    }

    private void Bind()
    {
        _view.OnMoveToUp
            .Subscribe(_ =>
            {
                _model.MoveToUp();
            })
            .AddTo(_disposable);

        _view.OnMoveToDown
            .Subscribe(_ =>
            {
                _model.MoveToDown();
            })
            .AddTo(_disposable);

        _view.OnSelect
            .Subscribe(_ =>
            {
                // 決定
                _model.ConfirmSelection();
                if (_model.IsFinishChoice())
                {
                    // Eventを終了
                    _view.FinishSelect();
                }
            })
            .AddTo(_disposable);

        _model.Choices
            .Subscribe(choices =>
            {
                _view.SetChoices(choices);
            })
            .AddTo(_disposable);

        _model.SelectedIndex
            .Subscribe(index =>
            {
                _view.SetSelectedIndex(index);
            })
            .AddTo(_disposable);

        _model.OnChoiceSelected
            .Subscribe(choice =>
            {
                TriggerNextEvent(choice);
            })
            .AddTo(_disposable);
    }

    private void TriggerNextEvent(Choice choice)
    {
        if (choice.NextEvent == null)
        {
            Debug.LogError("_nextEventがnullです。");
            return;
        }

        AbstractEvent nextEvent = choice.NextEvent.GetComponent<AbstractEvent>();
        if (nextEvent == null)
        {
            Debug.LogError("AbstractEventがアタッチされていません。");
            return;
        }
        nextEvent.TriggerEventForce();
    }

    ~ChoiceTextEventPresenter()
    {
        _disposable?.Dispose();
    }
}
