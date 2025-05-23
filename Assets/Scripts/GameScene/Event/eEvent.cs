/// <summary>
/// イベントの種類
/// </summary>
public enum eEvent
{
    None,
    TestEvent,
    MapMoveEvent,   // マップ移動イベント
    ItemEvent,      // アイテムイベント
    SpawnEnemyEvent, // 敵生成イベント
    
    // 1日目 Dream
    CreateEntranceKeyWithMedicine,  // 薬品で玄関の鍵を作成する
    Floors2And3StairsPasswordGimmick    // 階段のパスワードギミック
}