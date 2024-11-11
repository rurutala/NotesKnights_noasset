using UnityEngine;
using UnityEngine.UI;

public class HoldUnit : MonoBehaviour
{
    public int slotID;       // スロットID
    public int unitID = -1;  // キャラクターID（-1は未選択状態を示す）
    public string unitName;

    public GameObject uniton;
    public Image unitImage;  // キャラクター画像
    public Button selectButton; // スロット選択ボタン

    private void Start()
    {
        selectButton.onClick.AddListener(OnSelect);
    }

    // ボタンが押された時の処理
    private void OnSelect()
    {
        // 自分自身を送信
        UnitSelectManager.Instance.UnitsBuyOn(this);
    }

    // キャラクター画像とIDを更新
    public void UpdateUnit(int newID, Sprite newImage,string name)
    {
        if(newID == -1)
        {
            uniton.SetActive(false);
            unitID = -1;
            unitName = name;
            unitImage.sprite = null;
            return;
        }
        unitName = name;
        uniton.SetActive(true);
        unitID = newID;
        unitImage.sprite = newImage;
    }
}
