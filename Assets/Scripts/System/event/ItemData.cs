using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "Shop/ItemData")]
public class ItemData : ScriptableObject
{
    public int itemID;          // �A�C�e���̈�ӂ�ID
    public string itemName;     // �A�C�e����
    public string itemEffect;   // ���ʂ̐���
    public Sprite itemIcon;     // �A�C�e���̉摜
    public int money;
}