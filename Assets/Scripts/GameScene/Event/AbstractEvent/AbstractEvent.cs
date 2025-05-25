using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public abstract class AbstractEvent : MonoBehaviour
{
    [Header("イベントの設定")]
    // イベントの種類
    [SerializeField] private eEvent _event;

    [Header("1回だけ実行する")]
    // i回だけトリガーするか
    [SerializeField] private bool _isTriggeredOnce;

    // イベントの状態
    private eEventStatus _eventStatus = eEventStatus.NotTriggered;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        OnUpdateEvent();

        if (_isTriggeredOnce && _eventStatus == eEventStatus.Triggered)
        {
            BasicAnimation _basicAnimation = GetComponent<BasicAnimation>();
            if (_basicAnimation == null)
            {
                Debug.LogError("BasicAnimationがアタッチされていません");
            }
            _basicAnimation.Enabled = false; // 1回だけ実行する場合はアニメーションを無効にする
            return;
        }

        // イベントが実行中はトリガーしない
        if (_eventStatus != eEventStatus.Running)
        {
            if (IsTriggerEvent())
            {
                SetIsUnitMove(false); // Unitの移動を無効にする

                TriggerEvent();
                _eventStatus = eEventStatus.Running;
            }
        }

        if (IsFinishEvent() && _eventStatus != eEventStatus.Triggered)
        {
            SetIsUnitMove(true); // Unitの移動を有効にする

            _eventStatus = eEventStatus.Triggered;

#if DEBUG_MODE
            Debug.Log($"イベント: {_event} が終了しました");
#endif
        }
    }

    /// <summary>
    /// Unitの移動を有効/無効にする
    /// </summary>
    /// <param name="isUnitMove"> 有効/無効 </param>
    private void SetIsUnitMove(bool isUnitMove)
    {
        // 全てのUnitのUnitMoveを取得
        List<UnitMove> units = new List<UnitMove>(FindObjectsByType<UnitMove>(FindObjectsSortMode.None));

        foreach (UnitMove unit in units)
        {
            unit.IsEnabled = isUnitMove;
        }
    }

    /// <summary>
    /// イベントの更新処理(Update()の代わり)
    /// </summary>
    public virtual void OnUpdateEvent()
    {
    }

    /// <summary>
    /// イベントのトリガーの条件
    /// </summary>
    /// <returns> イベントをトリガーするか </returns>
    public abstract bool IsTriggerEvent();

    /// <summary>
    /// イベントをトリガーする
    /// </summary>
    public abstract void TriggerEvent();

    /// <summary>
    /// イベントが終了したか
    /// </summary>
    /// <returns> 終了したか </returns>
    public abstract bool IsFinishEvent();

    /// <summary>
    /// イベントの種類(ReadOnly)
    /// </summary>
    protected eEvent Event
    {
        get { return _event; }
    }

    /// <summary>
    /// イベントの状態(ReadOnly)
    /// </summary>
    protected eEventStatus EventStatus
    {
        get { return _eventStatus; }
    }
}
