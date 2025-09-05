/// <summary>
/// イベントの種類
/// </summary>
public enum eEvent
{
    None,
    TestEvent,
    MapMoveEvent,   // マップ移動イベント
    ItemEvent,      // アイテムイベント
    SpawnEnemyEvent, // 敵スポーンイベント
    TextEvent, // テキスト表示イベント
    DarkEvent,      // 暗転イベント
    SetDateEvent,   // 日付を設定するイベント
    PrologueEvent,  // プロローグイベント（ホラー用）

    // 1階層 Dream
    CreateEntranceKeyWithMedicine,  // 薬品で玄関への鍵を作成する
    Floors2And3StairsPasswordGimmick    // 階段のパスワードギミック
}
