using UnityEngine;

// ゲームの進行・操作を管理するクラス

public class GameController : MonoBehaviour
{
    public SaveManager saveManager;
    private GameData currentGameData;

    void Start()
    {
        // 1. ゲーム開始時に読み込み
        currentGameData = saveManager.LoadGame();

        // 2. データの操作例（コインを増やしたり、武器を所持にしたり）
        currentGameData.coins += 100;
        currentGameData.weapons[0].isOwned = true;
        currentGameData.weapons[0].levelA = 5;
    }

    // ボタンなどを押した時に実行する想定
    public void OnSaveButtonClick()
    {
        saveManager.SaveGame(currentGameData);
    }
}