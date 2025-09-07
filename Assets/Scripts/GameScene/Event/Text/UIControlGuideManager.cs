using UnityEngine;
using UnityEngine.UI;

public class UIControlGuideManager : MonoBehaviour
{
    [Header("操作説明UI")]
    [SerializeField] private Text controlGuideText;

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

    /// UI参照をタグで検索して取得
    private void FindUIObjectsByTag()
    {
        // UIタグが付いたオブジェクトをすべて取得
        targetUIs = GameObject.FindGameObjectsWithTag("UI");
    }

    /// 操作説明の表示状態を更新
    private void UpdateControlGuideVisibility()
    {
        bool shouldHide = IsAnyTargetUIActive();
        SetTextVisibility(!shouldHide);
    }

    /// テキストの表示状態を設定
    private void SetTextVisibility(bool visible)
    {
        if (controlGuideText != null)
        {
            controlGuideText.gameObject.SetActive(visible);
        }
    }

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

 
    public void RefreshUIReferences()
    {
        FindUIObjectsByTag();
    }
}
