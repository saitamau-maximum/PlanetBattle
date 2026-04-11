using System;
using System.Collections.Generic;

// ゲームのセーブデータを表すクラス

[Serializable]
public class EquipmentData 
{
    public bool isOwned;
    public int levelA;
    public int levelB;

    public EquipmentData()
    {
        isOwned = false;
        levelA = 0;
        levelB = 0;
    }
}

[Serializable]
public class GameData 
{
    public int coins;
    public int exp;
    public List<EquipmentData> weapons = new List<EquipmentData>();
    public List<EquipmentData> items = new List<EquipmentData>();

    public GameData()
    {
        coins = 0;
        exp = 0;
        for (int i = 0; i < 10; i++) weapons.Add(new EquipmentData());
        for (int i = 0; i < 10; i++) items.Add(new EquipmentData());
    }
}