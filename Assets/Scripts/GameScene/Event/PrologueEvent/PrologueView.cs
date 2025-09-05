using UnityEngine;
using TMPro;
using UnityEngine.UI;
using R3;
using System;

/// <summary>
/// プロローグビュー - UI表示を管理
/// </summary>
public class PrologueView : MonoBehaviour
{
    [Header("UI要素")]
    [SerializeField] private TextMeshProUGUI _textDisplay;
    [SerializeField] private Image _backgroundImage;

    // R3でのInput検知
    private Subject<Unit> _inputSubject = new Subject<Unit>();
    public Observable<Unit> OnInputDetected => _inputSubject.AsObservable();

    public void Initialize()
    {
        // UI要素の初期化
        if (_textDisplay != null)
        {
            _textDisplay.text = "";
            // テキストを透明に
            var textColor = _textDisplay.color;
            textColor.a = 0f;
            _textDisplay.color = textColor;
        }

        if (_backgroundImage != null)
        {
            _backgroundImage.color = Color.black;
            // 背景を透明に
            var bgColor = _backgroundImage.color;
            bgColor.a = 0f;
            _backgroundImage.color = bgColor;
        }

        // R3でInput監視を開始
        SetupInputObservable();
    }

    private void SetupInputObservable()
    {
        // Update()をR3のObservableに変換してInput検知
        Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return))
            .Subscribe(_ => _inputSubject.OnNext(Unit.Default))
            .AddTo(this);
    }

    public void UpdateText(string text)
    {
        if (_textDisplay != null)
        {
            _textDisplay.text = text;
        }
    }

    public void UpdateFade(float alpha)
    {
        // テキストのアルファ値を設定
        if (_textDisplay != null)
        {
            var textColor = _textDisplay.color;
            textColor.a = alpha;
            _textDisplay.color = textColor;
        }

        // 背景のアルファ値を設定
        if (_backgroundImage != null)
        {
            var bgColor = _backgroundImage.color;
            bgColor.a = alpha;
            _backgroundImage.color = bgColor;
        }
    }

    private void OnDestroy()
    {
        _inputSubject?.Dispose();
    }
}
