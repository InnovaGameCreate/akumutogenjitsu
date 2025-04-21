using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class TextDisplay : AbstractEvent
{
    [SerializeField] private GameObject textPanelPrefab;
    [SerializeField] private int linesPerPage = 2;
    [SerializeField, TextArea(3, 10)] private string message;

    private GameObject panelInstance;
    private Text textComponent;
    private Button panelButton;

    private List<string> textLines = new List<string>();
    private int currentPage = 0;
    private bool playerInRange = false;
    private bool isDisplaying = false;

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
        if (panelInstance == null)
        {
            panelInstance = Instantiate(textPanelPrefab);
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas != null)
            {
                panelInstance.transform.SetParent(canvas.transform, false);
            }

            textComponent = panelInstance.GetComponentInChildren<Text>();
            panelButton = panelInstance.GetComponent<Button>();
            if (panelButton != null)
            {
                panelButton.onClick.AddListener(NextPage);
            }
        }

        textLines = new List<string>(message.Split('\n'));
        currentPage = 0;
        isDisplaying = true;

        DisplayPage();
    }

    public override bool IsFinishEvent()
    {
        return !isDisplaying;
    }

    public override void OnUpdateEvent()
    {
        // ‚È‚µ
    }

    private void DisplayPage()
    {
        if (textComponent == null) return;

        int startLine = currentPage * linesPerPage;
        int endLine = Mathf.Min(startLine + linesPerPage, textLines.Count);
        textComponent.text = string.Join("\n", textLines.GetRange(startLine, endLine - startLine));
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
        if (textComponent != null)
            textComponent.text = "";

        textLines.Clear();
        currentPage = 0;

        if (panelInstance != null)
        {
            Destroy(panelInstance);
            panelInstance = null;
        }
    }
}
