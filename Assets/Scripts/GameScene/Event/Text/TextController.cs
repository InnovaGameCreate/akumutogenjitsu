using UnityEngine;

public class TextController : MonoBehaviour
{
    [SerializeField] private TextDisplay textDisplay;

    private void Start()
    {
        string message = "Here is 1行目\nこれは2行目\nこれは3行目\nこれは4行目\nこれは5行目\nこれは6行目";
        textDisplay.SetText(message);
    }
}
