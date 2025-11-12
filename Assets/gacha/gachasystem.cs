// ファイル名: GachaSystem_10.cs
using System;
using System.Collections.Generic;

// ※このスクリプトはUnityのオブジェクトにアタッチしない

// 抽選アイテムの情報を保持する構造体
public struct GachaItem
{
    public string Name;
    public int Weight;
    public int CumulativeWeight;
}

public class GachaSystem_10
{
    private List<GachaItem> Items;
    private Random Rnd;
    private int TotalWeight = 1000;

    public GachaSystem_10()
    {
        Rnd = new Random();
        
        // 1. 抽選データの設定 (省略)
        var rawItems = new List<(string Name, int Weight)>
        {
            ("SSR-1", 25), ("SSR-2", 25), ("SR-1", 50), ("SR-2", 50),
            ("SR-3", 50), ("R-1", 160), ("R-2", 160), ("R-3", 160),
            ("R-4", 160), ("R-5", 160)
        };

        // 2. 累積確率の計算 (省略)
        Items = new List<GachaItem>();
        int currentCumulative = 0;
        foreach (var item in rawItems)
        {
            currentCumulative += item.Weight;
            Items.Add(new GachaItem
            {
                Name = item.Name,
                Weight = item.Weight,
                CumulativeWeight = currentCumulative
            });
        }
    }

    // ガチャを1回引く
    public string Draw()
    {
        int roll = Rnd.Next(1, TotalWeight + 1);
        foreach (var item in Items)
        {
            if (roll <= item.CumulativeWeight)
            {
                return item.Name;
            }
        }
        return "エラー";
    }
}