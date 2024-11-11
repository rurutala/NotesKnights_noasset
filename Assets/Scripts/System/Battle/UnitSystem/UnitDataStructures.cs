using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitData
{
    public int unitID;       // ユニット固有のID
    public string unitName;  // ユニットの名前
    public Sprite unitIcon;  // UIに表示するためのアイコン
}

[System.Serializable]
public class PlayerDeck
{
    public List<UnitData> units; // プレイヤーが選択したユニットのリスト
}
