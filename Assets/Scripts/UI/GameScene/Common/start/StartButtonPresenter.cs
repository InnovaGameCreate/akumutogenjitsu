using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using R3;

public class StartButtonPresenter : MonoBehaviour
{
    [Header("設定")]
    [SerializeField] private string gameSceneName = "GameScene";
    [SerializeField] private CanvasGroup fadeCanvas;
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

        if (fadeCanvas != null)
        {
            float startTime = Time.time;
            var asyncOp = SceneManager.LoadSceneAsync(gameSceneName);
            asyncOp.allowSceneActivation = false;

            Observable.EveryUpdate()
                .TakeWhile(_ => Time.time - startTime < fadeDuration)
                .Subscribe(_ => fadeCanvas.alpha = (Time.time - startTime) / fadeDuration)
                .AddTo(_disposable);

            Observable.Timer(System.TimeSpan.FromSeconds(fadeDuration))
                .Subscribe(_ => asyncOp.allowSceneActivation = true)
                .AddTo(_disposable);
        }
        else
        {
            SceneManager.LoadSceneAsync(gameSceneName);
        }
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void OnDestroy()
    {
        _disposable.Dispose();
    }
}
