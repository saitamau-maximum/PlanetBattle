using System.IO;
using UnityEngine;

// ゲームのセーブとロードを管理するクラス

public class SaveManager : MonoBehaviour
{
    private string filePath;

    void Awake()
    {
        // 保存先のパスを設定（ファイル名は任意）
        filePath = Application.persistentDataPath + "/savedata.json";
    }

    // 保存処理
    public void SaveGame(GameData data)
    {
        // オブジェクトをJSON文字列に変換
        string json = JsonUtility.ToJson(data, true); // trueにすると見やすく整形される
        
        // ファイルに書き込み
        File.WriteAllText(filePath, json);
        Debug.Log("Saved to: " + filePath);
    }

    // 読み込み処理
    public GameData LoadGame()
    {
        if (File.Exists(filePath))
        {
            // ファイルからJSON文字列を読み込む
            string json = File.ReadAllText(filePath);
            
            // JSONをオブジェクトに復元
            return JsonUtility.FromJson<GameData>(json);
        }
        else
        {
            Debug.LogWarning("Save file not found. Creating new data.");
            return new GameData(); // ファイルがない場合は新規データを作成
        }
    }
}