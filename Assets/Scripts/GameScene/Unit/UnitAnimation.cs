using System.Collections.Generic;
using UnityEngine;

public class UnitAnimation : MonoBehaviour
{
    [Header("SpriteRenderer")]
    // 画像を表示する
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [Header("アニメーション用画像")]
    [SerializeField] private Sprite[] _sprites;
    private int _spritePerDirection;

    [Header("移動するUnitのUnitController")]
    // Unitの移動状態
    [SerializeField] private AbstractUnitController _unitController;


    [Header("移動するUnitMove")]
    [SerializeField] private UnitMove _unitMove;

    private float _previousSpeed;

    // Spriteが変わる時間
    private float _spriteChangeTime;
    // 経った時間
    private float _time = 0f;

    // 現在のSpriteのインデックス
    private int _currentSpriteIndex = 0;

    private static readonly Dictionary<eDirection, int> SpriteFirstIndexs = new Dictionary<eDirection, int>
    {
        { eDirection.Left, 6 },
        { eDirection.Right, 3 },
        { eDirection.Up, 9 },
        { eDirection.Down, 0 }
    };

    private eDirection _direction = eDirection.Down;

    /// <summary>
    /// 初期化処理を行う。
    /// 必要なコンポーネントの取得やスプライト数の計算を行う。
    /// </summary>
    void Start()
    {
        _spriteRenderer ??= GetComponent<SpriteRenderer>();
        _unitController ??= GetComponent<AbstractUnitController>();
        _unitMove ??= GetComponent<UnitMove>();

#if DEBUG_MODE
        if (_spriteRenderer == null || _unitController == null || _unitMove == null)
        {
            Debug.LogError($"必要なコンポーネントが不足しています: " +
                           $"{(_spriteRenderer == null ? "SpriteRenderer " : "")}" +
                           $"{(_unitController == null ? "UnitController " : "")}" +
                           $"{(_unitMove == null ? "UnitMove" : "")}");
        }

        if (_sprites == null || _sprites.Length == 0)
        {
            Debug.LogError("アニメーション用のスプライトが設定されていません。");
        }
        else if (_sprites.Length % 4 != 0)
        {
            Debug.LogError("スプライトの数が4方向に均等に分割できません。");
        }
#endif

        // 4方向なのでスプライトの数を4で割ると方向ごとのスプライト数になる
        _spritePerDirection = _sprites.Length / 4;

        _previousSpeed = _unitMove.Speed;

        UpdateSpriteChangeTime();
    }

    /// <summary>
    /// 毎フレーム呼び出される処理。
    /// ユニットの移動方向や速度に応じてアニメーションを更新する。
    /// </summary>
    void Update()
    {
        eDirection direction = GetCurrentDirection();

        // 入力なしのとき
        if (direction == eDirection.None)
        {
            StopAnimation();
            return;
        }

        // 方向が同じとき
        if (direction == _direction)
        {
            UpdateAnimationIndex(direction);
        }
        else
        {
            ChangeAnimationDirection(direction);
        }

        // 適用
        UpdateAnimation(_currentSpriteIndex);
    }

    /// <summary>
    /// 現在の移動方向を取得する。
    /// </summary>
    /// <returns>現在の移動方向を表す eDirection。</returns>
    private eDirection GetCurrentDirection()
    {
        UnitMoveStatus moveStatus = _unitController.unitMoveStatus;

        eDirection direction = eDirection.None;
        if (moveStatus.Left)
        {
            direction = eDirection.Left;
        }
        if (moveStatus.Right)
        {
            direction = eDirection.Right;
        }
        if (moveStatus.Up)
        {
            direction = eDirection.Up;
        }
        if (moveStatus.Down)
        {
            direction = eDirection.Down;
        }

        return direction;
    }

    /// <summary>
    /// アニメーションのインデックスを更新する。
    /// </summary>
    /// <param name="direction">現在の移動方向。</param>
    private void UpdateAnimationIndex(eDirection direction)
    {
        if (!CanUpdateAnimation())
        {
            return;
        }

        if (_currentSpriteIndex < SpriteFirstIndexs[direction] + _spritePerDirection - 1)
        {
            _currentSpriteIndex++;
        }
        else
        {
            _currentSpriteIndex = SpriteFirstIndexs[direction];
        }
    }

    /// <summary>
    /// アニメーションの方向を変更する。
    /// </summary>
    /// <param name="direction">新しい移動方向。</param>
    private void ChangeAnimationDirection(eDirection direction)
    {
        _currentSpriteIndex = SpriteFirstIndexs[direction];
        _direction = direction;
    }

    /// <summary>
    /// アニメーションを停止する。
    /// </summary>
    private void StopAnimation()
    {
        // 動き終わったら止まっている状態にする
        _spriteRenderer.sprite = _sprites[SpriteFirstIndexs[_direction]];

        // 初動はアニメーションを更新する
        _time = _spriteChangeTime;
    }

    /// <summary>
    /// 指定されたインデックスのスプライトを適用する。
    /// </summary>
    /// <param name="index">スプライトのインデックス。</param>
    private void UpdateAnimation(int index)
    {
        _spriteRenderer.sprite = _sprites[index];
    }

    /// <summary>
    /// アニメーションを更新できるかを判定する。
    /// </summary>
    /// <returns>更新可能な場合は true、それ以外は false。</returns>
    private bool CanUpdateAnimation()
    {
        UpdateSpriteChangeTime();

        if (_time < _spriteChangeTime)
        {
            _time += Time.deltaTime;
            return false;
        }
        _time = 0f;

        return true;
    }

    /// <summary>
    /// スプライトが切り替わる時間を更新する。
    /// </summary>
    private void UpdateSpriteChangeTime()
    {
        if (_unitMove.Speed != _previousSpeed)
        {
            _spriteChangeTime = _unitMove.Speed * 0.05f;
            _previousSpeed = _unitMove.Speed;
        }
    }
}
