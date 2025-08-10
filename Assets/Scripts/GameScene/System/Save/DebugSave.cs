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
        Debug.Log("Save");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("保存できました。");
            _saveMgr.SaveToFile(1);
        }
    }
}