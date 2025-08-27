using UnityEngine;

/// <summary>
/// BaseUIのMVPパターン実装のエントリーポイント
/// 互換性のために残しているレガシークラス
/// 新しい実装はBaseUIPresenterを使用してください
/// </summary>
public class BaseUI : MonoBehaviour
{
    // BaseUIPresenterへの参照は後で追加
    // [SerializeField] private BaseUIPresenter _presenter;

    [Header("後方互換性のための値")]
    [SerializeField] private int _month = 9;
    [SerializeField] private int _day = 10;
    [SerializeField] private eWorldType _worldType = eWorldType.Real;
    [SerializeField] private string _place;

    void Start()
    {
        Debug.Log("BaseUI: MVP パターンへの移行中です。BaseUIPresenterを使用してください。");
    }

    // 後方互換性のためのプロパティ
    public int Month
    {
        get => _month;
        set => _month = value;
    }

    public int Day
    {
        get => _day;
        set => _day = value;
    }

    public string Place
    {
        get => _place;
        set => _place = value;
    }

    public eWorldType WorldType
    {
        get => _worldType;
        set => _worldType = value;
    }

    // 新しいAPIメソッド
    public void SetDate(int month, int day)
    {
        _month = month;
        _day = day;
    }
}
