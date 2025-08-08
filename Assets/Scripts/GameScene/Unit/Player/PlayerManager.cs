using UnityEngine;
using VContainer;

class PlayerManager : MonoBehaviour, ISaveableManager<PlayerSaveData>
{
    [Inject]
    private UnitMove _playerUnitMove;

    public PlayerSaveData EncodeToSaveData()
    {
        PlayerSaveData saveData = new PlayerSaveData();
        saveData.Position = _playerUnitMove.transform.position;
        return saveData;
    }

    public void LoadFromSaveData(PlayerSaveData saveData)
    {
        _playerUnitMove.transform.position = saveData.Position;
    }
}
