using UnityEngine;
using System.Collections.Generic;
using R3;

[System.Serializable]
public class TextLine
{
    [Header("話者")]
    public CharacterData characterData;

    [Header("メッセージ")]
    [TextArea(3, 5)]
    public string Message;

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
    [SerializeField] private GameObject textBoxPrefab;
    [SerializeField] private List<TextLine> textLines = new List<TextLine>();

    private Canvas targetCanvas;
    private GameObject panelInstance;
    private TextBoxUI textBoxUI;
    private int currentLineIndex = 0;
    private bool playerInRange = false;
    private bool isDisplaying = false;

    public override void OnStartEvent()
    {
        targetCanvas = GameObject.FindGameObjectWithTag("UICanvas")?.GetComponent<Canvas>();
        if (targetCanvas == null)
        {
            Debug.LogError("[TextEvent] UICanvas not found!");
            return;
        }

        // PlayerInput購読（トリガー用）
        PlayerInput.Instance.OnPerformed(PlayerInput.Instance.Input.Base.Interact)
            .Where(ctx => ctx.ReadValueAsButton() && playerInRange && !isDisplaying)
            .Subscribe(_ =>
            {
                onTriggerEvent.OnNext(Unit.Default);
            })
            .AddTo(_disposable);

        // PlayerInput購読（テキスト送り用）
        PlayerInput.Instance.OnPerformed(PlayerInput.Instance.Input.TextEvent.NextPage)
            .Where(ctx => ctx.ReadValueAsButton() && isDisplaying && EventStatus == eEventStatus.Running)
            .Subscribe(_ =>
            {
                NextLine();
            })
            .AddTo(_disposable);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    public override void TriggerEvent()
    {
        // ✅ すでに表示中なら何もしない
        if (isDisplaying)
        {
            return;
        }

        if (!PlayerInput.Instance.Input.TextEvent.enabled)
        {
            PlayerInput.Instance.Input.Base.Disable();
            PlayerInput.Instance.Input.TextEvent.Enable();
        }

        if (textLines == null || textLines.Count == 0)
        {
            Debug.LogWarning("[TextEvent] textLines is null or empty!");
            return;
        }

        if (panelInstance == null)
        {
            panelInstance = Instantiate(textBoxPrefab);

            Canvas canvas = targetCanvas != null ? targetCanvas : FindFirstObjectByType<Canvas>();
            if (canvas != null)
            {
                panelInstance.transform.SetParent(canvas.transform, false);
            }

            textBoxUI = panelInstance.GetComponent<TextBoxUI>();
            if (textBoxUI == null)
            {
                Debug.LogError("[TextEvent] TextBoxUI component not found!");
                return;
            }
        }

        isDisplaying = true;
        DisplayCurrentLine();
    }

    public override void OnFinishEvent()
    {
        PlayerInput.Instance.Input.TextEvent.Disable();
        PlayerInput.Instance.Input.Base.Enable();
        isDisplaying = false;
    }

    private void DisplayCurrentLine()
    {
        if (textBoxUI == null || textLines == null) return;

        if (currentLineIndex >= textLines.Count)
        {
            ClearText();
            return;
        }

        TextLine currentLine = textLines[currentLineIndex];

        textBoxUI.Message = currentLine.Message;
        textBoxUI.Name = currentLine.GetCurrentSpeakerName();
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
            ClearText();
            onFinishEvent.OnNext(Unit.Default);
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

        isDisplaying = false;
    }

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
