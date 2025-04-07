using UnityEngine;

public class TextController : MonoBehaviour
{
    [SerializeField] private TextDisplay textDisplay;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            string message = "ここが1行目\n2行目\n3行目\n4行目\n5行目";
            textDisplay.SetText(message);
        }
    }

}
