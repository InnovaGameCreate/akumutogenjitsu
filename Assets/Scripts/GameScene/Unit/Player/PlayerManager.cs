using UnityEngine;
using VContainer;

public class PlayerManager : MonoBehaviour, ISaveableManager<PlayerSaveData>
{
    [Inject]
    private UnitMove _playerUnitMove;

    public PlayerSaveData EncodeToSaveData()
    {
        PlayerSaveData saveData = new PlayerSaveData();
        saveData.Position = new SerializableVector3(_playerUnitMove.transform.position);
        return saveData;
    }

    public void LoadFromSaveData(PlayerSaveData saveData)
    {
        if (saveData?.Position != null)
        {
            _playerUnitMove.transform.position = saveData.Position.ToVector3();
        }
    }
}
