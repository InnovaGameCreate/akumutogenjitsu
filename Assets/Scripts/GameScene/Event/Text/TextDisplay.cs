using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class TextDisplay : AbstractEvent, IPointerClickHandler
{
    [SerializeField] private Text textComponent; 
    [SerializeField] private int linesPerPage = 2;
    private List<string> textLines = new List<string>();
    private int currentPage = 0;
    private bool isTriggered = false;

    private void Awake()
    {
        if (textComponent == null)
        {
            textComponent = GetComponent<Text>();
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
            isTriggered = false;
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
        
    }

    public override bool IsTriggerEvent()
    {
        return isTriggered;
    }

    public override void TriggerEvent()
    {
        // Šù‚ÉƒgƒŠƒK[‚³‚ê‚Ä‚¢‚é‚È‚ç‰½‚à‚µ‚È‚¢
        if (isTriggered) return;
        isTriggered = true;
    }

    public override bool IsFinishEvent()
    {
        return !isTriggered;
    }

    public void ClearText()
    {
        textLines.Clear();
        textComponent.text = "";
        currentPage = 0;
        isTriggered = false;
    }
}