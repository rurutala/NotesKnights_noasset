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

        // �o�^���ꂽ���j�b�g�̃��X�g���f�o�b�O���O�Ŋm�F
        unitSelectionSystem.LogAllRegisteredUnits();
    }

    private Unit CreateUnitFromData(UnitData unitData)
    {
        // unitID��unitName��g�ݍ��킹�āA�v���n�u�����쐬
        string prefabPath = "prefabs/Player_Unit/" + unitData.unitID +"_"+ unitData.unitName;

        // �쐬�����v���n�u������Ƀv���n�u�����[�h
        GameObject unitPrefab = Resources.Load<GameObject>(prefabPath);
        if (unitPrefab == null)
        {
            Debug.LogError($"Prefab not found at path: {prefabPath}");
            return null; // �v���n�u��������Ȃ������ꍇ��null��Ԃ�
        }


        // �v���n�u���C���X�^���X��
        GameObject unitObject = Instantiate(unitPrefab);
        GameFlowManager.Instance.PlayerRegister(unitObject);

        // Unit�R���|�[�l���g�Ƀf�[�^��ݒ�
        Unit unitComponent = unitObject.GetComponent<Unit>();
        unitComponent.unitID = unitData.unitID;
        unitComponent.unitName = unitData.unitName;

        return unitComponent;
    }
}
