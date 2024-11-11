using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "Shop/ItemData")]
public class ItemData : ScriptableObject
{
    public int itemID;          // アイテムの一意のID
    public string itemName;     // アイテム名
    public string itemEffect;   // 効果の説明
    public Sprite itemIcon;     // アイテムの画像
    public int money;
}