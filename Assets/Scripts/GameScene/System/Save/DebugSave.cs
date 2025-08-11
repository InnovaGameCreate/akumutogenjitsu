using UnityEngine;
using VContainer;
using VContainer.Unity;

public class DebugSave : ITickable
{
    [Inject] private SaveManager _saveMgr;

    public DebugSave()
    {
        Debug.Log("DebugSaveが生成");
    }

    public void Tick()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("Load");
            _saveMgr.LoadFromFile(1);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Save");
            _saveMgr.SaveToFile(1);
        }
    }
}