using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class TextLine
{
    [Header("話者")]
    public CharacterData characterData;

    [Header("メッセージ")]
    [TextArea(3, 5)]
    public string Message;

    // プロパティで統一的にアクセス
    public string GetCurrentSpeakerName()
    {
        if (characterData != null)
            return characterData.CharacterName;
        return "";
    }

    public Sprite GetCurrentCharacterSprite()
    {
        if (characterData != null)
            return characterData.CharacterSprite;
        return null;
    }
}

public class TextEvent : AbstractEvent
{
    [SerializeField] private GameObject textBoxPrefab; // TextBoxUIコンポーネントを持つPrefab
    [SerializeField] private List<TextLine> textLines = new List<TextLine>(); // TextData.TextLinesを直接埋め込み


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

        if (textLines == null || textLines.Count == 0)
        {
            return;
        }

        if (panelInstance == null)
        {
            // TextBoxUIを持つPrefabをインスタンス化
            panelInstance = Instantiate(textBoxPrefab);

            Canvas canvas = targetCanvas != null ? targetCanvas : FindFirstObjectByType<Canvas>();
            if (canvas != null)
            {
                panelInstance.transform.SetParent(canvas.transform, false);
            }

            // TextBoxUIコンポーネントを取得
            textBoxUI = panelInstance.GetComponent<TextBoxUI>();
            if (textBoxUI == null)
            {
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
        if (textBoxUI == null || textLines == null) return;

        if (currentLineIndex >= textLines.Count)
        {
            isDisplaying = false;
            ClearText();
            return;
        }

        TextLine currentLine = textLines[currentLineIndex];

        // テキストと名前の更新
        textBoxUI.Message = currentLine.Message;
        textBoxUI.Name = currentLine.GetCurrentSpeakerName();

        // スプライト表示（コメントアウト解除済）
        textBoxUI.CharacterSprite = currentLine.GetCurrentCharacterSprite();
    }

    private void NextLine()
    {
        currentLineIndex++;

        if (currentLineIndex < textLines.Count)
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

    // エディタ用のヘルパーメソッド
    [ContextMenu("Add Text Line")]
    private void AddTextLine()
    {
        textLines.Add(new TextLine());
    }

    [ContextMenu("Clear All Lines")]
    private void ClearAllLines()
    {
        textLines.Clear();
    }
}