using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerDeck playerDeck;

    private void Start()
    {
        var unitSelectionSystem = FindObjectOfType<UnitSelectionSystem>();

        if(DataManager.Instance != null)
        playerDeck = DataManager.Instance.playerDeck;

        foreach (var unitData in playerDeck.units)
        {
            Unit newUnit = CreateUnitFromData(unitData);
            unitSelectionSystem.RegisterUnit(newUnit);
        }

        // 登録されたユニットのリストをデバッグログで確認
        unitSelectionSystem.LogAllRegisteredUnits();
    }

    private Unit CreateUnitFromData(UnitData unitData)
    {
        // unitIDとunitNameを組み合わせて、プレハブ名を作成
        string prefabPath = "prefabs/Player_Unit/" + unitData.unitID +"_"+ unitData.unitName;

        // 作成したプレハブ名を基にプレハブをロード
        GameObject unitPrefab = Resources.Load<GameObject>(prefabPath);
        if (unitPrefab == null)
        {
            Debug.LogError($"Prefab not found at path: {prefabPath}");
            return null; // プレハブが見つからなかった場合はnullを返す
        }


        // プレハブをインスタンス化
        GameObject unitObject = Instantiate(unitPrefab);
        GameFlowManager.Instance.PlayerRegister(unitObject);

        // Unitコンポーネントにデータを設定
        Unit unitComponent = unitObject.GetComponent<Unit>();
        unitComponent.unitID = unitData.unitID;
        unitComponent.unitName = unitData.unitName;

        return unitComponent;
    }
}
