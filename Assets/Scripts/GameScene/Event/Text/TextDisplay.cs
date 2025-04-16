using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class TextDisplay : AbstractEvent, IPointerClickHandler
{
    [SerializeField] private Text textComponent; // Legacy Text に変更
    [SerializeField] private int linesPerPage = 2;
    [SerializeField, TextArea(3, 10)] private string message; // 外部で設定可能に

    private List<string> textLines = new List<string>();
    private int currentPage = 0;
    private bool playerInRange = false;
    private bool isDisplaying = false;

    private void Awake()
    {
        if (textComponent == null)
        {
            textComponent = GetComponent<Text>();
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
        isDisplaying = true;
        DisplayPage();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (EventStatus == eEventStatus.Running && isDisplaying)
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
            isDisplaying = false;
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
        return playerInRange && Input.GetKeyDown(KeyCode.Z);
    }

    public override void TriggerEvent()
    {
        SetText(message);
    }

    public override bool IsFinishEvent()
    {
        return !isDisplaying;
    }

    public void ClearText()
    {
        textLines.Clear();
        textComponent.text = "";
        currentPage = 0;
    }
}
