using UnityEngine;
using R3;
using UnityEngine.SceneManagement;

public class MenuPresenter : MonoBehaviour
{
    [SerializeField] private MenuModel _model;
    [SerializeField] private MenuView _view;

    private readonly CompositeDisposable _disposable = new();

    void Start()
    {
        if (_model == null)
        {
            Debug.LogError("MenuModelがアサインされていません。");
            return;
        }
        if (_view == null)
        {
            Debug.LogError("MenuViewがアサインされていません。");
            return;
        }

        Bind();
    }

    private void Bind()
    {
        // View → Model
        _view.MoveLeft
            .Subscribe(_ =>
            {
                _model.MoveToLeft();
            })
            .AddTo(_disposable);

        _view.MoveRight
            .Subscribe(_ =>
            {
                _model.MoveToRight();
            })
            .AddTo(_disposable);

        // Select処理
        _view.Select
            .Subscribe(_ =>
            {
                int selectedIndex = _model.SelectedIndex.CurrentValue;
                switch (selectedIndex)
                {
                    case 0: // Save
                        Debug.Log("セーブメニューが選択されました");
                        _view.ShowSaveMenu(true);
                        _view.ActionMapToMenu(false);
                        _view.ActionMapToSave(true);
                        break;
                    case 1: // Title
                        GameObject essentialObj = GameObject.FindWithTag("EssentialObject");
                        GameObject playerObj = GameObject.FindWithTag("Player");
                        if (essentialObj != null)
                        {
                            Object.Destroy(essentialObj);
                        }
                        if (playerObj != null)
                        {
                            Object.Destroy(playerObj);
                        }
                        _disposable?.Dispose();
                        SceneManager.LoadScene("start");
                        break;
                }
            })
            .AddTo(_disposable);

        // Close処理
        _view.Close
            .Subscribe(_ =>
            {
                _view.ActionMapToMenu(false);

                var uiView = GameObject.FindWithTag("UICanvas")?.GetComponent<UIView>();
                if (uiView != null)
                {
                    uiView.OnMenuInput();
                }
            })
            .AddTo(_disposable);

        // Model → View
        _model.SelectedIndex
            .Subscribe(index =>
            {
                _view.SelectItem(index);
            })
            .AddTo(_disposable);
    }

    void OnDestroy()
    {
        _disposable.Dispose();
    }
}
