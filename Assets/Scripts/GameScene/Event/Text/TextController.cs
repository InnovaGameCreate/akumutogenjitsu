using UnityEngine;

public class TextController : MonoBehaviour
{
    [SerializeField] private TextDisplay textDisplay;

    private void Start()
    {
        string message = "Here is 1�s��\n�����2�s��\n�����3�s��\n�����4�s��\n�����5�s��\n�����6�s��";
        textDisplay.SetText(message);
    }
}
