using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnitSelection
{
    void SelectUnitByID(int unitID); // ���j�b�gID���g���ă��j�b�g��I��
    Unit GetSelectedUnit();          // ���ݑI�𒆂̃��j�b�g���擾
}

public class UnitSelectionSystem : MonoBehaviour, IUnitSelection
{
    public Dictionary<int, Unit> units = new Dictionary<int, Unit>();
    public Unit selectedUnit;
    private Unit beforeUnit;

    public void RegisterUnit(Unit unit)
    {
        if (!units.ContainsKey(unit.unitID))
        {
            units.Add(unit.unitID, unit);
            Debug.Log($"Unit Registered: ID = {unit.unitID}, Name = {unit.unitName}");
        }
        else
        {
            Debug.LogWarning($"Unit with ID {unit.unitID} is already registered.");
        }
    }

    public void SelectUnitByID(int unitID)
    {


        if (units.ContainsKey(unitID))
        {
            selectedUnit = units[unitID];
            Debug.Log($"{selectedUnit.unitName} has been selected.");
            if(beforeUnit == null)
            {
                beforeUnit = selectedUnit;
            }
            else
            {
                if (beforeUnit == selectedUnit) return;
                beforeUnit.MoveToNewPosition(new Vector3(800, 0, 0));
                beforeUnit = selectedUnit;
            }
        }
        else if (unitID == -1)
        {
            selectedUnit = null;
            beforeUnit = null;
        }
        else if (unitID == -2)
        {
            beforeUnit.MoveToNewPosition(new Vector3(800, 0, 0));
            selectedUnit = null;
            beforeUnit = null;

        }
        else
        {
            Debug.LogError($"Unit with ID {unitID} not found.");
        }
    }

    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }

    // ���ׂĂ̓o�^���ꂽ���j�b�g���f�o�b�O���O�Ŋm�F���郁�\�b�h
    public void LogAllRegisteredUnits()
    {
        Debug.Log("Logging all registered units:");
        foreach (var unit in units)
        {
            Debug.Log($"Unit ID: {unit.Key}, Name: {unit.Value.unitName}");
        }
    }
}
