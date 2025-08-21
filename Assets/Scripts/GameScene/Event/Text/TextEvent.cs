using UnityEngine;

public class TextEvent : AbstractEvent
{
    [SerializeField] private GameObject textBoxPrefab; // TextBoxUIコンポーネントを持つPrefab
    [SerializeField] private TextData textData; // 会話データ
    private Canvas targetCanvas; // 配置先のCanvas

    private GameObject panelInstance;
    private TextBoxUI textBoxUI;

    private int currentLineIndex = 0;
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
        
        if (textData == null || textData.TextLines == null || textData.TextLines.Length == 0)
        {
            Debug.LogError("TextDataが設定されていないか、会話データが空です。");
            return;
        }
        
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

        currentLineIndex = 0;
        isDisplaying = true;
        DisplayCurrentLine();
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
                NextLine();
            }
        }
    }

    private void DisplayCurrentLine()
    {
        if (textBoxUI == null || textData == null) return;

        if (currentLineIndex >= textData.TextLines.Length)
        {
            isDisplaying = false;
            ClearText();
            return;
        }

        TextLine currentLine = textData.TextLines[currentLineIndex];
        
        // TextBoxUIのプロパティを使用してテキストを更新
        textBoxUI.Message = currentLine.Message;
        textBoxUI.Name = currentLine.SpeakerName;
        
        // TODO: 立ち絵があれば表示
        // if (textBoxUI.HasCharacterSprite && currentLine.CharacterSprite != null)
        // {
        //     textBoxUI.CharacterSprite = currentLine.CharacterSprite;
        // }
    }

    private void NextLine()
    {
        currentLineIndex++;
        
        if (currentLineIndex < textData.TextLines.Length)
        {
            DisplayCurrentLine();
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

        currentLineIndex = 0;

        if (panelInstance != null)
        {
            Destroy(panelInstance);
            panelInstance = null;
            textBoxUI = null;
        }

        _hasFinished = true;
    }
}
