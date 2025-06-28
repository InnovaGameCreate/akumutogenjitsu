using UnityEngine;
using System.Collections.Generic;

public class TextEvent : AbstractEvent
{
    [SerializeField] private GameObject textBoxPrefab; // TextBoxUIコンポーネントを持つPrefab
    private Canvas targetCanvas; // 配置先のCanvas
    [SerializeField] private int linesPerPage = 2;
    [SerializeField, TextArea(3, 10)] private string message;
    [SerializeField] private string speakerName = ""; // 話者名

    private GameObject panelInstance;
    private TextBoxUI textBoxUI;

    private List<string> textLines = new List<string>();
    private int currentPage = 0;
    private bool playerInRange = false;
    private bool isDisplaying = false;

    private bool _hasFinished = false;

    public override void OnStartEvent()
    {
        targetCanvas = GameObject.FindGameObjectWithTag("UICanvas").GetComponent<Canvas>();
        if (targetCanvas == null)
        {
            Debug.LogError("Canvasがアサインされていません。");
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) playerInRange = false;
    }

    public override bool IsTriggerEvent()
    {
        return playerInRange && Input.GetKeyDown(KeyCode.Z);
    }

    public override void TriggerEvent()
    {
        // イベント開始時に必ず初期化
        _hasFinished = false;
        
        if (panelInstance == null)
        {
            // TextBoxUIを持つPrefabをインスタンス化
            panelInstance = Instantiate(textBoxPrefab);
            
            // 指定されたCanvasに配置、なければ自動検索
            Canvas canvas = targetCanvas != null ? targetCanvas : FindFirstObjectByType<Canvas>();
            if (canvas != null)
            {
                panelInstance.transform.SetParent(canvas.transform, false);
            }

            // TextBoxUIコンポーネントを取得
            textBoxUI = panelInstance.GetComponent<TextBoxUI>();
            if (textBoxUI == null)
            {
                Debug.LogError("TextBoxUIコンポーネントがPrefabに見つかりません。");
                return;
            }
        }

        textLines = new List<string>(message.Split('\n'));
        currentPage = 0;
        isDisplaying = true;
        DisplayPage();
    }

    public override bool IsFinishEvent()
    {
        if (EventStatus == eEventStatus.Triggered)
        {
            _hasFinished = false;
        }
        return _hasFinished;
    }

    public override void OnUpdateEvent()
    {
        if (EventStatus == eEventStatus.Running)
        {
            if (isDisplaying && (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return)))
            {
                NextPage();
            }
        }
    }

    private void DisplayPage()
    {
        if (textBoxUI == null) return;

        int startLine = currentPage * linesPerPage;
        int endLine = Mathf.Min(startLine + linesPerPage, textLines.Count);
        
        // TextBoxUIのプロパティを使用してテキストを更新
        textBoxUI.Message = string.Join("\n", textLines.GetRange(startLine, endLine - startLine));
        textBoxUI.Name = speakerName;
    }

    private void NextPage()
    {
        int nextStartLine = (currentPage + 1) * linesPerPage;
        if (nextStartLine < textLines.Count)
        {
            currentPage++;
            DisplayPage();
        }
        else
        {
            isDisplaying = false;
            ClearText();
        }
    }

    private void ClearText()
    {
        if (textBoxUI != null)
        {
            textBoxUI.Message = "";
            textBoxUI.Name = "";
        }

        textLines.Clear();
        currentPage = 0;

        if (panelInstance != null)
        {
            Destroy(panelInstance);
            panelInstance = null;
            textBoxUI = null;
        }

        _hasFinished = true;
    }
}
