using UnityEngine;
using UnityEngine.SceneManagement;
using R3;

/// <summary>
/// BaseUIと既存システムの統合を管理するヘルパークラス
/// </summary>
public static class BaseUISystemIntegration
{
    /// <summary>
    /// DateManagerの変更を監視するObservable
    /// </summary>
    public static Observable<Date> ObserveDateManager()
    {
        return Observable.Interval(System.TimeSpan.FromSeconds(0.1f))
            .Where(_ => DateManager.Instance != null)
            .Select(_ => DateManager.Instance.GetCurrentDate())
            .DistinctUntilChanged();
    }

    /// <summary>
    /// SceneLocationManagerの変更を監視するObservable
    /// </summary>
    public static Observable<string> ObserveSceneLocation()
    {
        return Observable.Interval(System.TimeSpan.FromSeconds(0.1f))
            .Where(_ => SceneLocationManager.Instance != null)
            .Select(_ => {
                string currentSceneName = SceneManager.GetActiveScene().name;
                return SceneLocationManager.Instance.GetLocationDisplayNameFromSceneName(currentSceneName);
            })
            .DistinctUntilChanged();
    }

    /// <summary>
    /// シーン変更時のイベントを監視するObservable
    /// </summary>
    public static Observable<string> ObserveSceneChange()
    {
        return Observable.FromEvent<UnityEngine.Events.UnityAction<Scene, Scene>, Scene>(
            h => (oldScene, newScene) => h(newScene),
            h => SceneManager.activeSceneChanged += h,
            h => SceneManager.activeSceneChanged -= h
        ).Select(scene => scene.name);
    }

    /// <summary>
    /// より効率的な場所監視（シーン変更時のみ更新）
    /// </summary>
    public static Observable<string> ObserveLocationOnSceneChange()
    {
        // 初期値を設定してからシーン変更を監視
        var initialLocation = GetCurrentLocationName();
        
        return Observable.Return(initialLocation)
            .Concat(ObserveSceneChange()
                .Where(_ => SceneLocationManager.Instance != null)
                .Select(sceneName => SceneLocationManager.Instance.GetLocationDisplayNameFromSceneName(sceneName))
            );
    }
    
    /// <summary>
    /// 現在の場所名を取得するヘルパーメソッド
    /// </summary>
    private static string GetCurrentLocationName()
    {
        if (SceneLocationManager.Instance != null)
        {
            return SceneLocationManager.Instance.GetLocationDisplayNameFromSceneName(SceneManager.GetActiveScene().name);
        }
        return "不明なシーン";
    }
}
