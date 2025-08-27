using UnityEngine;
using UnityEngine.SceneManagement;
using R3;

public class BaseUIModel : MonoBehaviour
{
    [Header("初期値")]
    [SerializeField] private eWorldType _initialWorldType = eWorldType.Real;

    private readonly ReactiveProperty<Date> _currentDate = new();
    private readonly ReactiveProperty<string> _currentPlace = new();
    private readonly ReactiveProperty<eWorldType> _worldType = new();

    public ReadOnlyReactiveProperty<Date> CurrentDate => _currentDate;
    public ReadOnlyReactiveProperty<string> CurrentPlace => _currentPlace;
    public ReadOnlyReactiveProperty<eWorldType> WorldType => _worldType;

    void Start()
    {
        Initialize();
        StartObservingSystemChanges();
    }

    private void Initialize()
    {
        _worldType.Value = _initialWorldType;
        
        // DateManagerから現在の日付を取得
        UpdateDateFromManager();
        
        // SceneLocationManagerから現在の場所を取得
        UpdatePlaceFromManager();
    }

    private void StartObservingSystemChanges()
    {
        // DateManagerの変更を監視
        BaseUISystemIntegration.ObserveDateManager()
            .Subscribe(date => _currentDate.Value = date)
            .AddTo(this);

        // SceneLocationManagerの変更を監視（シーン変更時のみ）
        BaseUISystemIntegration.ObserveLocationOnSceneChange()
            .Subscribe(place => _currentPlace.Value = place)
            .AddTo(this);
    }

    private void UpdateDateFromManager()
    {
        if (DateManager.Instance != null)
        {
            Date newDate = DateManager.Instance.GetCurrentDate();
            if (!Date.IsSameDate(_currentDate.Value, newDate))
            {
                _currentDate.Value = newDate;
            }
        }
    }

    private void UpdatePlaceFromManager()
    {
        if (SceneLocationManager.Instance != null)
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            string newPlace = SceneLocationManager.Instance.GetLocationDisplayNameFromSceneName(currentSceneName);
            
            if (_currentPlace.Value != newPlace)
            {
                _currentPlace.Value = newPlace;
            }
        }
    }

    // 外部からワールドタイプを変更するためのメソッド
    public void SetWorldType(eWorldType worldType)
    {
        _worldType.Value = worldType;
    }

    // 手動で更新を強制するメソッド
    public void ForceUpdate()
    {
        UpdateDateFromManager();
        UpdatePlaceFromManager();
    }

    void OnDestroy()
    {
        _currentDate?.Dispose();
        _currentPlace?.Dispose();
        _worldType?.Dispose();
    }
}
