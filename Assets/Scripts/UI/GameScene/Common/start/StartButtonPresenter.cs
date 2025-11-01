using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using R3;
using DG.Tweening;

public class StartButtonPresenter : MonoBehaviour
{
    [Header("設定")]
    [SerializeField] private string _gameSceneName = "GameScene";
    [SerializeField] private Image _fadeImage;
    [SerializeField] private float _fadeDuration = 0.5f;

    [SerializeField] private StartButtonModel _model;
    [SerializeField] private StartButtonView _view;

    private readonly CompositeDisposable _disposable = new();
    private bool _isTransitioning = false;

    void Start()
    {
        if (_model == null || _view == null)
        {
            Debug.LogError("ModelまたはViewがアサインされていません。");
            return;
        }
        Bind();
    }

    private void Bind()
    {
        // View → Model
        _view.MoveUp.Subscribe(_ => _model.MoveLeft()).AddTo(_disposable);
        _view.MoveDown.Subscribe(_ => _model.MoveRight()).AddTo(_disposable);

        _view.SelectItem.Subscribe(index =>
        {
            if (index >= 0) _model.SetSelectedIndex(index); // 直接選択
            ExecuteCurrentAction(); // 実行
        }).AddTo(_disposable);

        // Model → View
        _model.SelectedIndex.Subscribe(index => _view.UpdateSelection(index)).AddTo(_disposable);
    }

    private void ExecuteCurrentAction()
    {
        // if (_isTransitioning) return;

        switch (_model.SelectedIndex.CurrentValue)
        {
            case 0:
                StartGame(); break;

            case 1:
                LoadScene(); break;

            case 2:
                QuitGame(); break;
        }
    }

    private void StartGame()
    {
        _isTransitioning = true;

        if (_fadeImage != null)
        {
            // フェード用Imageをアクティブにする
            _fadeImage.gameObject.SetActive(true);

            // シーン読み込み開始
            var asyncOp = SceneManager.LoadSceneAsync(_gameSceneName);
            asyncOp.allowSceneActivation = false;

            // 透明から開始
            Color color = _fadeImage.color;
            _fadeImage.color = new Color(color.r, color.g, color.b, 0f);

            // DOTweenでフェードアウト
            _fadeImage.DOFade(1f, _fadeDuration)
                .SetEase(Ease.OutQuad)
                .OnComplete(() => asyncOp.allowSceneActivation = true);
        }
        else
        {
            SceneManager.LoadScene(_gameSceneName);
        }
    }

    private void LoadScene()
    {
        _isTransitioning = true;

        if (_fadeImage != null)
        {
            // フェード用Imageをアクティブにする
            _fadeImage.gameObject.SetActive(true);

            // シーン読み込み開始
            var asyncOp = SceneManager.LoadSceneAsync("Load");
            asyncOp.allowSceneActivation = false;

            // 透明から開始
            Color color = _fadeImage.color;
            _fadeImage.color = new Color(color.r, color.g, color.b, 0f);

            // DOTweenでフェードアウト
            _fadeImage.DOFade(1f, _fadeDuration)
                .SetEase(Ease.OutQuad)
                .OnComplete(() => asyncOp.allowSceneActivation = true);
        }
        else
        {
            SceneManager.LoadScene("Load");
        }
    }

    private System.Collections.IEnumerator FadeAndLoadScene()
    {
        yield return null;
    }

    private void QuitGame()
    {
        ApplicationService.QuitApplication();
    }

    void OnDestroy()
    {
        _disposable.Dispose();
    }
}
