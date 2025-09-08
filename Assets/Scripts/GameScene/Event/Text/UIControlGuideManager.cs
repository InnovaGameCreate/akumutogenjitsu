using UnityEngine;
using TMPro;

public class UIControlGuideManager : MonoBehaviour
{
    [Header("操作説明UI")]
    [SerializeField] private TextMeshProUGUI controlGuideText;

    // 自動検索されるUI配列
    private GameObject[] targetUIs;

    void Start()
    {
        // UI参照をタグで検索
        FindUIObjectsByTag();
        // 初期状態の設定
        UpdateControlGuideVisibility();
    }

    void Update()
    {
        // UI参照を毎フレーム更新（新しいUIの検出）
        FindUIObjectsByTag();
        // 毎フレームUIの状態をチェック
        UpdateControlGuideVisibility();
    }

    /// <summary>
    /// UI参照をタグで検索して取得
    /// </summary>
    private void FindUIObjectsByTag()
    {
        // UIタグが付いたオブジェクトをすべて取得
        targetUIs = GameObject.FindGameObjectsWithTag("UI");
    }

    /// <summary>
    /// 操作説明の表示状態を更新
    /// </summary>
    private void UpdateControlGuideVisibility()
    {
        bool shouldHide = IsAnyTargetUIActive();
        SetTextVisibility(!shouldHide);
    }

    /// <summary>
    /// テキストの表示状態を設定
    /// </summary>
    private void SetTextVisibility(bool visible)
    {
        if (controlGuideText != null)
        {
            controlGuideText.gameObject.SetActive(visible);
        }
    }

    /// <summary>
    /// 対象UIのいずれかがアクティブかチェック
    /// </summary>
    private bool IsAnyTargetUIActive()
    {
        if (targetUIs == null) return false;

        foreach (var ui in targetUIs)
        {
            if (ui != null && ui.activeInHierarchy)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// UI参照を手動で更新
    /// </summary>
    public void RefreshUIReferences()
    {
        FindUIObjectsByTag();
    }
}
