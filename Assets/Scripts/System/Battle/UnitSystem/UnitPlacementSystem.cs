using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnitPlacement
{
    void PlaceUnit(Unit unit, Vector3 mousePosition);
    void ConfirmPlacement(Unit unit);
    void RemoveUnit(Unit unit); // ���j�b�g�̍폜
}

public class UnitPlacementSystem : MonoBehaviour, IUnitPlacement
{
    private LayerMask gridLayerMask;

    public float yOffset = 1.0f;  // y�������̃I�t�Z�b�g�l

    private HashSet<Vector3> occupiedPositions = new HashSet<Vector3>();
    private Dictionary<int, GameObject> placedUnits = new Dictionary<int, GameObject>();

    private Vector3 currentPlacementPosition;
    private bool isUnitPlaced = false;

    private void Start()
    {
        gridLayerMask = LayerMask.GetMask("Grid");
    }

    public void PlaceUnit(Unit unit, Vector3 mousePosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, gridLayerMask))
        {
            Vector3 gridPosition = hit.collider.bounds.center;

            // tag��"Wall"�̏ꍇ�AyOffset��2�{�ɂ���
            float adjustedYOffset = yOffset;
            bool isOnWall = hit.collider.CompareTag("Wall");
            bool isOnGround = hit.collider.CompareTag("Ground");
            if (isOnWall)
            {
                adjustedYOffset += (yOffset-0.25f);
                Debug.Log("Wall�Ƀq�b�g�ByOffset���{�ɂ��܂����B");
            }

            currentPlacementPosition = gridPosition + new Vector3(0, adjustedYOffset, 0);

            // Unit��onWall�t���O�Ɋ�Â��z�u����
            if (unit.onWall && isOnWall)
            {
                // ���j�b�g��onWall����Wall�^�O�̈ʒu�ɉ��z�u
                if (!occupiedPositions.Contains(currentPlacementPosition))
                {
                    unit.transform.position = currentPlacementPosition;
                    isUnitPlaced = true;
                    Debug.Log($"{unit.unitName}��{currentPlacementPosition}��Wall�ɉ��z�u����܂���");
                    TimeManager.Instance.TimeScaleChange(0.3f);
                }
                else
                {
                    Debug.Log("����Wall�}�X�ɂ͊��Ƀ��j�b�g���z�u����Ă��܂�");
                }
            }
            else if (!unit.onWall && !isOnWall && isOnGround)
            {
                // ���j�b�g��onWall�łȂ�����Wall�ȊO�̏ꏊ�ɉ��z�u
                if (!occupiedPositions.Contains(currentPlacementPosition))
                {
                    unit.transform.position = currentPlacementPosition;
                    isUnitPlaced = true;
                    Debug.Log($"{unit.unitName}��{currentPlacementPosition}�ɉ��z�u����܂���");
                    TimeManager.Instance.TimeScaleChange(0.3f);
                }
                else
                {
                    Debug.Log("���̃}�X�ɂ͊��Ƀ��j�b�g���z�u����Ă��܂�");
                }
            }
            else
            {
                // �z�u�����ɍ���Ȃ��ꍇ
                if (unit.onWall)
                {
                    Debug.Log("���̃��j�b�g��Wall�̏�ɂ����z�u�ł��܂���");
                }
                else
                {
                    Debug.Log("���̃��j�b�g��Wall�̏�ɂ͔z�u�ł��܂���");
                }
            }
        }
    }


    public void RemoveUnit(Unit unit)
    {
        if (placedUnits.ContainsKey(unit.unitID))
        {
            Vector3 position = placedUnits[unit.unitID].transform.position;
            occupiedPositions.Remove(position); // �ʒu���J��
            placedUnits.Remove(unit.unitID);

            Debug.Log($"{unit.unitName}���폜����܂����B{position}�͍Ăї��p�\�ł��B");
        }
    }

    public void ConfirmPlacement(Unit unit)
    {
        if (isUnitPlaced && !occupiedPositions.Contains(currentPlacementPosition))
        {
            // �ŏI�z�u�F�ʒu���L�^���Ċm�肷��
            occupiedPositions.Add(currentPlacementPosition);
            placedUnits[unit.unitID] = unit.gameObject;

            // ���j�b�g���ݒu���ꂽ���Ƃ�ʒm
            unit.StartLifetimeCountdown();

            ConfirmPlacement();
            Debug.Log($"{unit.unitName}��{currentPlacementPosition}�ɍŏI�I�ɔz�u����܂���");
            CostManager.Instance.ConsumeCost(unit.unitcost);
            isUnitPlaced = false;
        }
        else
        {
            Debug.Log("���̃}�X�ɂ͊��Ƀ��j�b�g���z�u����Ă��܂�");
        }
    }


    public void ConfirmPlacement()
    {
        TimeManager.Instance.TimeScaleChange(1f);
    }

    public bool IsUnitAlive(int unitID)
    {
        if (placedUnits.ContainsKey(unitID))
        {
            return placedUnits[unitID] != null;
        }
        return false;
    }

}
