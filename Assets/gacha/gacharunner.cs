// ファイル名: GachaRunner.cs
using UnityEngine; // Unityの機能を使うために必要

// ※このスクリプトをUnityのオブジェクトにアタッチする

public class GachaRunner : MonoBehaviour
{
    // GachaSystem_10 を保持する変数
    private GachaSystem_10 gachaSystem;

    // ゲーム開始時に一度だけ呼ばれる (Unityの実行ボタンを押した時)
    void Start()
    {
        // 1. ガチャシステムを初期化（使えるように準備）
        gachaSystem = new GachaSystem_10();

        Debug.Log("ガチャのテスト実行を開始します...");

        // 2. 試しに10回引いてみる
        for (int i = 0; i < 10; i++)
        {
            // ガチャを実行して結果を取得
            string result = gachaSystem.Draw();
            
            // 3. Unityの「コンソール」に結果を出力
            Debug.Log($"ガチャの結果 ({i+1}回目): {result}");
        }
    }
}