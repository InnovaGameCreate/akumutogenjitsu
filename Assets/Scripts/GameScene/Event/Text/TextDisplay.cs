using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class TextDisplay : AbstractEvent, IPointerClickHandler
{
    [SerializeField] private Text textComponent; // Legacy Text に変更
    [SerializeField] private int linesPerPage = 2;
    [SerializeField] private string messageText; // 外部から設定可能なメッセージ

    private List<string> textLines = new List<string>();
    private int currentPage = 0;
    private bool isTriggered = false;
    private bool playerInRange = false;

    private void Awake()
    {
        if (textComponent == null)
        {
            textComponent = GetComponent<Text>();
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.Z))
        {
            if (!isTriggered)
            {
                TriggerEvent();
            }
        }
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

    public void SetText(string message)
    {
        textLines = new List<string>(message.Split('\n'));
        currentPage = 0;
        isTriggered = true;
        DisplayPage();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isTriggered)
        {
            NextPage();
        }
    }

    private void NextPage()
    {
        if ((currentPage + 1) * linesPerPage < textLines.Count)
        {
            currentPage++;
            DisplayPage();
        }
        else
        {
            ClearText();
        }
    }

    private void DisplayPage()
    {
        if (textComponent == null) return;

        int startLine = currentPage * linesPerPage;
        int endLine = Mathf.Min(startLine + linesPerPage, textLines.Count);
        textComponent.text = string.Join("\n", textLines.GetRange(startLine, endLine - startLine));
    }

    public override void OnUpdateEvent()
    {
        // 特に更新処理なし
    }

    public override bool IsTriggerEvent()
    {
        return false; // TriggerEventで直接管理する
    }

    public override void TriggerEvent()
    {
        if (isTriggered) return;

        // 外部から設定されるのを待つだけにする
        isTriggered = true;
        DisplayPage();
    }

    public override bool IsFinishEvent()
    {
        return !isTriggered;
    }

    public void ClearText()
    {
        textLines.Clear();
        if (textComponent != null)
            textComponent.text = "";
        currentPage = 0;
        isTriggered = false;
    }
}
