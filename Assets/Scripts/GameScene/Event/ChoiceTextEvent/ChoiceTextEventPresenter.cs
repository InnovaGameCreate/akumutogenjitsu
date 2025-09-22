using System.Collections.Generic;
using UnityEngine;
using R3;
using System.Threading.Tasks;

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

        _model.Message
            .Subscribe(message =>
            {
                _view.SetMessage(message);
            })
            .AddTo(_disposable);

        _model.OnChoiceSelected
            .Subscribe(async choice =>
            {
                await TriggerNextEvent(choice);
                Debug.Log("awaitが終了しました。");
            })
            .AddTo(_disposable);
    }

    private async Task TriggerNextEvent(Choice choice)
    {
        // Viewを再表示
        if (choice.IsReturn)
        {
            await ReturnChoiceEvent();
            return;
        }
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

    private async Task ReturnChoiceEvent()
    {
        _view.gameObject.SetActive(false);
        await Task.Delay(500);

        _view.gameObject.SetActive(true);
    }

    ~ChoiceTextEventPresenter()
    {
        _disposable?.Dispose();
    }
}
