using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using R3;
using DG.Tweening;

public class StartButtonPresenter : MonoBehaviour
{
    [Header("設定")]
    [SerializeField] private string gameSceneName = "GameScene";
    [SerializeField] private Image fadeImage; 
    [SerializeField] private float fadeDuration = 0.5f;

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
        _view.MoveLeft.Subscribe(_ => _model.MoveLeft()).AddTo(_disposable);
        _view.MoveRight.Subscribe(_ => _model.MoveRight()).AddTo(_disposable);

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
        if (_isTransitioning) return;

        int currentIndex = _model.SelectedIndex.CurrentValue;
        if (currentIndex == 0) StartGame();
        else QuitGame();
    }

    private void StartGame()
    {
        _isTransitioning = true;

        if (fadeImage != null)
        {
            // フェード用Imageをアクティブにする
            fadeImage.gameObject.SetActive(true);

            // シーン読み込み開始
            var asyncOp = SceneManager.LoadSceneAsync(gameSceneName);
            asyncOp.allowSceneActivation = false;

            // 透明から開始
            Color color = fadeImage.color;
            fadeImage.color = new Color(color.r, color.g, color.b, 0f);

            // DOTweenでフェードアウト
            fadeImage.DOFade(1f, fadeDuration)
                .SetEase(Ease.OutQuad)
                .OnComplete(() => asyncOp.allowSceneActivation = true);
        }
        else
        {
            SceneManager.LoadSceneAsync(gameSceneName);
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
