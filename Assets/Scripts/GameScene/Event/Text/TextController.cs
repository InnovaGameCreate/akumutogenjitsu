using UnityEngine;

public class TextController : MonoBehaviour
{
    [SerializeField] private TextDisplay textDisplay;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            string message = "������1�s��\n2�s��\n3�s��\n4�s��\n5�s��";
            textDisplay.SetText(message);
        }
    }

}
