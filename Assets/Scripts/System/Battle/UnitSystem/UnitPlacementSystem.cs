using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnitPlacement
{
    void PlaceUnit(Unit unit, Vector3 mousePosition);
    void ConfirmPlacement(Unit unit);
    void RemoveUnit(Unit unit); // ユニットの削除
}

public class UnitPlacementSystem : MonoBehaviour, IUnitPlacement
{
    private LayerMask gridLayerMask;

    public float yOffset = 1.0f;  // y軸方向のオフセット値

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

            // tagが"Wall"の場合、yOffsetを2倍にする
            float adjustedYOffset = yOffset;
            bool isOnWall = hit.collider.CompareTag("Wall");
            bool isOnGround = hit.collider.CompareTag("Ground");
            if (isOnWall)
            {
                adjustedYOffset += (yOffset-0.25f);
                Debug.Log("Wallにヒット。yOffsetを二倍にしました。");
            }

            currentPlacementPosition = gridPosition + new Vector3(0, adjustedYOffset, 0);

            // UnitのonWallフラグに基づく配置条件
            if (unit.onWall && isOnWall)
            {
                // ユニットがonWallかつWallタグの位置に仮配置
                if (!occupiedPositions.Contains(currentPlacementPosition))
                {
                    unit.transform.position = currentPlacementPosition;
                    isUnitPlaced = true;
                    Debug.Log($"{unit.unitName}が{currentPlacementPosition}にWallに仮配置されました");
                    TimeManager.Instance.TimeScaleChange(0.3f);
                }
                else
                {
                    Debug.Log("このWallマスには既にユニットが配置されています");
                }
            }
            else if (!unit.onWall && !isOnWall && isOnGround)
            {
                // ユニットがonWallでないかつWall以外の場所に仮配置
                if (!occupiedPositions.Contains(currentPlacementPosition))
                {
                    unit.transform.position = currentPlacementPosition;
                    isUnitPlaced = true;
                    Debug.Log($"{unit.unitName}が{currentPlacementPosition}に仮配置されました");
                    TimeManager.Instance.TimeScaleChange(0.3f);
                }
                else
                {
                    Debug.Log("このマスには既にユニットが配置されています");
                }
            }
            else
            {
                // 配置条件に合わない場合
                if (unit.onWall)
                {
                    Debug.Log("このユニットはWallの上にしか配置できません");
                }
                else
                {
                    Debug.Log("このユニットはWallの上には配置できません");
                }
            }
        }
    }


    public void RemoveUnit(Unit unit)
    {
        if (placedUnits.ContainsKey(unit.unitID))
        {
            Vector3 position = placedUnits[unit.unitID].transform.position;
            occupiedPositions.Remove(position); // 位置を開放
            placedUnits.Remove(unit.unitID);

            Debug.Log($"{unit.unitName}が削除されました。{position}は再び利用可能です。");
        }
    }

    public void ConfirmPlacement(Unit unit)
    {
        if (isUnitPlaced && !occupiedPositions.Contains(currentPlacementPosition))
        {
            // 最終配置：位置を記録して確定する
            occupiedPositions.Add(currentPlacementPosition);
            placedUnits[unit.unitID] = unit.gameObject;

            // ユニットが設置されたことを通知
            unit.StartLifetimeCountdown();

            ConfirmPlacement();
            Debug.Log($"{unit.unitName}が{currentPlacementPosition}に最終的に配置されました");
            CostManager.Instance.ConsumeCost(unit.unitcost);
            isUnitPlaced = false;
        }
        else
        {
            Debug.Log("このマスには既にユニットが配置されています");
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
