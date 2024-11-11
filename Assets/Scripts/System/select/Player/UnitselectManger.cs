using UnityEngine;
using System.Collections.Generic;

public class UnitSelectManager : MonoBehaviour
{
    public static UnitSelectManager Instance { get; private set; }

    public GameObject Unitsbuy;
    public GameObject chooseunit;

    public int currentselectunitid;
    public string currentselectunitname;

    public HoldUnit selectedHoldUnit; // 現在選択されているHoldUnit
    public List<HoldUnit> holdUnits = new List<HoldUnit>(); // HoldUnitのリスト

    public BuyUnit selected_unit;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // ユニット購入画面を表示
    public void UnitsBuyOn(HoldUnit holdUnit)
    {
        selectedHoldUnit = holdUnit; // 選択されたHoldUnitを保存
        Unitsbuy.SetActive(true);
        // 二枚目の画面を表示（キャラクター選択画面）
        // ここで既に選択されているキャラクターを無効化
    }

    // 詳細画面を表示
    public void UnitsBuyDetailOn(int unitID, string name, BuyUnit unit)
    {
        selected_unit = unit;
        chooseunit.SetActive(true);
        currentselectunitid = unitID;
        currentselectunitname = name;
        // 三枚目の画面を表示
        // ユニットの詳細を表示
    }

    // キャラクター選択を確定しパーティを更新
    public void ConfirmUnitSelection()
    {
        int newID = currentselectunitid;
        // 他のスロットで同じIDが使われていたらそちらを解除
        foreach (var holdUnit in holdUnits)
        {
            if (holdUnit.unitID == newID && holdUnit != selectedHoldUnit)
            {
                holdUnit.UpdateUnit(-1, null,null); // IDを-1にしてUIを更新
            }
        }

        // 選択スロットに新IDをセット
        selectedHoldUnit.UpdateUnit(newID, GetUnitSprite(newID),currentselectunitname); // UIを更新

        // パーティ情報を更新
        UpdatePartyCharacterIDs();

        Unitsbuy.SetActive(false);
        chooseunit.SetActive(false);
    }
    // パーティキャラIDリストをHoldUnitから更新
    private void UpdatePartyCharacterIDs()
    {
        List<int> updatedPartyIDs = new List<int>(new int[holdUnits.Count]);

        for (int i = 0; i < holdUnits.Count; i++)
        {
            updatedPartyIDs[i] = holdUnits[i].unitID;
        }

        // DataManagerにパーティ情報を更新
        DataManager.Instance.UpdatePartyCharacterIDs(updatedPartyIDs);

    }


    // 指定したIDが既に選択されているか確認
    public bool IsUnitSelected(int unitID)
    {
        return selectedHoldUnit.unitID == unitID;
    }

    // 指定IDのユニット画像を取得
    public Sprite GetUnitSprite(int unitID)
    {
        // "Resources/image/ui/Unit" + unitIDという形式でスプライトをロード
        string spritePath = "image/ui/Unit" + unitID;
        Sprite unitSprite = Resources.Load<Sprite>(spritePath);

        // スプライトが見つからなかった場合の対処
        if (unitSprite == null)
        {
            Debug.LogError($"Sprite not found at path: {spritePath}");
            return null;
        }

        return unitSprite;
    }

    public void bought()
    {
        DataManager.Instance.moneyunit -= (int)selected_unit.buyCost;
        selected_unit.buyCost = 0;
    }

    public void UnitDataSend()
    {
        DataManager.Instance.SetHoldUnits(holdUnits);
    }
}
