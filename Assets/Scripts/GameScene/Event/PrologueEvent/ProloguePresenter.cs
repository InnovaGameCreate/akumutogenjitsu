using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using R3;

/// <summary>
/// プロローグプレゼンター - ModelとViewを接続し、ビジネスロジックを管理
/// </summary>
public class ProloguePresenter : MonoBehaviour
{
    private PrologueModel _model;
    private PrologueView _view;
    private Coroutine _typewriterCoroutine;
    private bool _skipToEnd = false;

    public void Initialize(List<PrologueEvent.PrologueLine> prologueLines)
    {
        _model = new PrologueModel();
        _view = GetComponent<PrologueView>();

        if (_view == null)
        {
            Debug.LogError("[ProloguePresenter] PrologueViewが見つかりません。");
            return;
        }

        _model.Initialize(prologueLines);
        _view.Initialize();

        // Model変更時のView更新
        _model.CurrentText.Subscribe(text => _view.UpdateText(text)).AddTo(this);
        _model.FadeAlpha.Subscribe(alpha => _view.UpdateFade(alpha)).AddTo(this);

        // ViewからのInput通知をR3で受け取る
        _view.OnInputDetected
            .Subscribe(_ => _model.TriggerInput())
            .AddTo(this);

        // ModelのInput処理をリアクティブに
        _model.OnInputReceived
            .Subscribe(_ => HandleInputKey())
            .AddTo(this);
    }

    public void StartPrologue()
    {
        StartCoroutine(PrologueCoroutine());
    }

    public bool IsFinished()
    {
        return _model?.IsFinished.Value ?? false;
    }

    private void HandleInputKey()
    {
        if (_model.IsTyping.Value)
        {
            // タイプライター効果中なら全文表示
            _skipToEnd = true;
        }
        else if (_model.WaitingForInput.Value)
        {
            // 入力待ち状態なら次の行へ
            _model.SetWaitingForInput(false);
        }
    }

    private IEnumerator PrologueCoroutine()
    {
        // フェードイン
        _model.SetFadeAlpha(0f);
        DOTween.To(() => _model.FadeAlpha.Value, value => _model.SetFadeAlpha(value), 1f, 0.5f);
        yield return new WaitForSeconds(0.5f);

        // 各行を順番に表示
        while (_model.HasValidCurrentLine())
        {
            var currentLine = _model.GetCurrentLine();

            // 表示前の待機
            if (currentLine.delayBeforeDisplay > 0)
            {
                yield return new WaitForSeconds(currentLine.delayBeforeDisplay);
            }

            // タイプライター効果
            _typewriterCoroutine = StartCoroutine(TypewriterEffect(currentLine));
            yield return _typewriterCoroutine;

            // 入力待ち状態に
            _model.SetWaitingForInput(true);

            // キー入力待ち
            yield return new WaitUntil(() => !_model.WaitingForInput.Value);

            // 表示後の待機
            if (currentLine.delayAfterDisplay > 0)
            {
                yield return new WaitForSeconds(currentLine.delayAfterDisplay);
            }

            _model.MoveToNextLine();
        }

        // フェードアウト
        DOTween.To(() => _model.FadeAlpha.Value, value => _model.SetFadeAlpha(value), 0f, 0.5f);
        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator TypewriterEffect(PrologueEvent.PrologueLine line)
    {
        string fullText = line.text;
        _model.SetIsTyping(true);
        _skipToEnd = false;

        for (int i = 0; i <= fullText.Length; i++)
        {
            _model.SetCurrentText(fullText.Substring(0, i));

            // スキップ要求があったら全文表示して終了
            if (_skipToEnd)
            {
                _model.SetCurrentText(fullText);
                break;
            }

            if (i < fullText.Length)
            {
                yield return new WaitForSeconds(line.typeSpeed);
            }
        }

        _model.SetIsTyping(false);
    }

    private void OnDestroy()
    {
        // R3のAddTo(this)により自動的に購読解除されるため、手動解除は不要
        _model?.Dispose();
    }
}
