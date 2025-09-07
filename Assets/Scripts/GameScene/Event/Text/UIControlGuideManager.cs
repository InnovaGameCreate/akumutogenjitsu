using UnityEngine;
using UnityEngine.UI;

public class UIControlGuideManager : MonoBehaviour
{
    [Header("操作説明UI")]
    [SerializeField] private Text controlGuideText; // TextMeshProの場合は適宜変更

    [Header("監視対象のUI名前")]
    [SerializeField] private string inventoryUIName = "InventoryUI";
    [SerializeField] private string menuUIName = "MenuUI";
    [SerializeField] private string saveUIName = "SaveUI"; //TextBoxUIで使用

    [Header("監視対象のUI（自動検索）")]
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject menuUI;
    [SerializeField] private GameObject saveUI;

    [Header("設定")]
    [SerializeField] private bool hideText = true; // テキストだけを非表示にするか

    void Start()
    {
        // UI参照を名前で検索
        FindUIObjectsByName();

        // 初期状態の設定
        UpdateControlGuideVisibility();
    }

    void Update()
    {
        // UI参照が失われている場合は再検索
        RefreshUIReferences();

        // 毎フレームUI状態をチェックして表示を更新
        UpdateControlGuideVisibility();
    }

    /// UI参照を名前で検索して取得
    private void FindUIObjectsByName()
    {
        // InventoryUIを検索
        if (!string.IsNullOrEmpty(inventoryUIName))
        {
            inventoryUI = FindUIByName(inventoryUIName);
        }

        // MenuUIを検索
        if (!string.IsNullOrEmpty(menuUIName))
        {
            menuUI = FindUIByName(menuUIName);
        }

        // TextBoxUIを検索
        if (!string.IsNullOrEmpty(saveUIName))
        {
            saveUI = FindUIByName(saveUIName);
        }
    }

    /// <summary>
    /// 名前でUIオブジェクトを検索（(Clone)付きも対応）
    /// </summary>
    /// <param name="uiName">検索するUI名</param>
    /// <returns>見つかったGameObject、なければnull</returns>
    private GameObject FindUIByName(string uiName)
    {
        try
        {
            // 完全一致で検索
            GameObject found = GameObject.Find(uiName);
            if (found != null) return found;

            // (Clone)付きで検索
            found = GameObject.Find(uiName + "(Clone)");
            if (found != null) return found;

            // 部分一致で検索（より柔軟な検索）
            GameObject[] allObjects = FindObjectsOfType<GameObject>();
            foreach (var obj in allObjects)
            {
                if (obj != null && obj.name.Contains(uiName))
                {
                    return obj;
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"UI検索でエラーが発生しました: {e.Message}");
        }

        return null;
    }

    /// <summary>
    /// UI参照を定期的に更新（オブジェクトが破棄・再生成された場合の対応）
    /// </summary>
    private void RefreshUIReferences()
    {
        // 参照が失われている場合は再検索
        if (inventoryUI == null && !string.IsNullOrEmpty(inventoryUIName))
        {
            inventoryUI = FindUIByName(inventoryUIName);
        }

        if (menuUI == null && !string.IsNullOrEmpty(menuUIName))
        {
            menuUI = FindUIByName(menuUIName);
        }

        if (saveUI == null && !string.IsNullOrEmpty(saveUIName))
        {
            saveUI = FindUIByName(saveUIName);
        }
    }

    /// <summary>
    /// 操作説明の表示状態を更新
    /// </summary>
    private void UpdateControlGuideVisibility()
    {
        bool shouldHide = IsAnyTargetUIActive();

        // テキストの表示制御
        if (hideText && controlGuideText != null)
        {
            controlGuideText.gameObject.SetActive(!shouldHide);
        }
    }

    /// <summary>
    /// 監視対象のUIのいずれかがアクティブかどうかをチェック
    /// </summary>
    /// <returns>いずれかのUIがアクティブならtrue</returns>
    private bool IsAnyTargetUIActive()
    {
        // InventoryUIの確認
        if (inventoryUI != null && inventoryUI.activeInHierarchy)
        {
            return true;
        }

        // MenuUIの確認
        if (menuUI != null && menuUI.activeInHierarchy)
        {
            return true;
        }

        // SaveUIの確認
        if (saveUI != null && saveUI.activeInHierarchy)
        {
            return true;
        }

        return false;
    }

    /// 外部からUI名前を設定
    public void SetInventoryUIName(string uiName)
    {
        inventoryUIName = uiName;
        inventoryUI = FindUIByName(uiName);
    }

    public void SetMenuUIName(string uiName)
    {
        menuUIName = uiName;
        menuUI = FindUIByName(uiName);
    }

    public void SetSaveUIName(string uiName)
    {
        saveUIName = uiName;
        saveUI = FindUIByName(uiName);
    }

    /// 外部からUI参照を直接設定
    public void SetInventoryUI(GameObject ui)
    {
        inventoryUI = ui;
    }

    public void SetMenuUI(GameObject ui)
    {
        menuUI = ui;
    }

    public void SetSaveUI(GameObject ui)
    {
        saveUI = ui;
    }

    /// 操作説明UIの参照を設定

    public void SetControlGuideText(Text text)
    {
        controlGuideText = text;
    }

    /// 設定の変更
    public void SetHideText(bool hide)
    {
        hideText = hide;
        UpdateControlGuideVisibility();
    }

    /// 手動でUI検索を実行 
    public void RefreshAllUIReferences()
    {
        FindUIObjectsByName();
    }
}
