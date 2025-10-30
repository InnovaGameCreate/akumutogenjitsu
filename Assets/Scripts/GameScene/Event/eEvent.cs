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
    ChoiceTextEvent,    // 選択肢を表示するイベント
    PasswordEvent,  // パスワードを表示するイベント
    DarkEvent,      // 暗転イベント
    SetDateEvent,   // 日付を設定するイベント
    PrologueEvent,  // プロローグイベント（ホラー用）
    UnitMoveEvent,  // ユニットを動かすイベント
    CreateObjectEvent,  // オブジェクトを生成するイベント
    DeleteObjectEvent,  // オブジェクトを削除するイベント
    BgmEvent,           // BGMを変更するイベント
    SeEvent,            // SEを流すイベント

    // 1階層 Dream
    CreateEntranceKeyWithMedicine,  // 薬品で玄関への鍵を作成する
    Floors2And3StairsPasswordGimmick,    // 階段のパスワードギミック
    UseEntranceKeyEvent,    // 鍵を使って玄関から出るギミック
}
