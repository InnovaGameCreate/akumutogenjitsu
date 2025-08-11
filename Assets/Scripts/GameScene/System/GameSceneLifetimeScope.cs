using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameSceneLifetimeScope : LifetimeScope
{
    private SystemManager _systemMgr;
    private EventManager _eventMgr;
    private ItemManager _itemMgr;
    private DateManager _dateMgr;
    private PlayerManager _playerMgr;

    protected override void Configure(IContainerBuilder builder)
    {
        if (_systemMgr != null) 
        {
            builder.RegisterComponent(_systemMgr);
        }
        else
        {
            Debug.LogError("SystemManagerがnullです！");
        }

        if (_eventMgr != null) 
        {
            builder.RegisterComponent(_eventMgr);
        }
        else
        {
            Debug.LogError("EventManagerがnullです！");
        }

        if (_itemMgr != null) 
        {
            builder.RegisterComponent(_itemMgr);
        }
        else
        {
            Debug.LogError("ItemManagerがnullです！");
        }

        if (_dateMgr != null) 
        {
            builder.RegisterComponent(_dateMgr);
        }
        else
        {
            Debug.LogError("DateManagerがnullです！");
        }

        if (_playerMgr != null) 
        {
            builder.RegisterComponent(_playerMgr);
        }
        else
        {
            Debug.LogError("PlayerManagerがnullです！");
        }

        var unitMove = FindFirstObjectByType<UnitMove>();
        if (unitMove != null)
        {
            builder.RegisterComponent(unitMove);
        }
        else
        {
            Debug.LogError("UnitMoveが見つかりません！");
        }

        builder.Register<SaveManager>(Lifetime.Singleton);

        builder.RegisterEntryPoint<DebugSave>();
    }

    protected override void Awake()
    {
        var systemMgrObj = GameObject.FindWithTag("SystemMgr");
        var eventMgrObj = GameObject.FindWithTag("EventMgr");
        var itemMgrObj = GameObject.FindWithTag("ItemMgr");
        var dateMgrObj = GameObject.FindWithTag("DateMgr");
        var playerMgrObj = GameObject.FindWithTag("PlayerMgr");

        if (systemMgrObj == null)
        {
            Debug.LogError("SystemMgrが存在しませんでした。");
            return;
        }
        if (eventMgrObj == null)
        {
            Debug.LogError("EventMgrが存在しませんでした。");
            return;
        }
        if (itemMgrObj == null)
        {
            Debug.LogError("ItemMgrが存在しませんでした。");
            return;
        }
        if (dateMgrObj == null)
        {
            Debug.LogError("DateMgrが存在しませんでした。");
            return;
        }
        if (playerMgrObj == null)
        {
            Debug.LogError("PlayerMgrが存在しませんでした。");
            return;
        }

        _systemMgr = systemMgrObj.GetComponent<SystemManager>();
        _eventMgr = eventMgrObj.GetComponent<EventManager>();
        _itemMgr = itemMgrObj.GetComponent<ItemManager>();
        _dateMgr = dateMgrObj.GetComponent<DateManager>();
        _playerMgr = playerMgrObj.GetComponent<PlayerManager>();

        base.Awake();
    }
}