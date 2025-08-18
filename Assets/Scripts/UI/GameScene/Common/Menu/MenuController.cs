using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] private MenuUI _view;

    [Header("メニューの項目数")]
    [SerializeField] private int _menuNum = 2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _view.SelectedIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateButton();
    }

    private void UpdateButton()
    {
        int index = _view.SelectedIndex;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (index <= 0) return;

            index--;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (index >= _menuNum - 1) return;
            index++;
        }
        _view.SelectedIndex = index;
    }
}
