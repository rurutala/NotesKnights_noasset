using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnitRotation
{
    void StartRotation();
    void EndRotation();
}
public class UnitRotationSystem : MonoBehaviour, IUnitRotation
{
    private Vector3 startPos;
    private Vector3 endPos;
    private bool isRotating = false;
    private Unit selectedUnit;



    public void StartRotation()
    {
        // マウスの位置を記録
        startPos = Input.mousePosition;
        isRotating = true;

        // 現在選択されているユニットを取得
        selectedUnit = FindObjectOfType<UnitSelectionSystem>().GetSelectedUnit();
    }

    public void EndRotation()
    {
        if (isRotating && selectedUnit != null)
        {
            // マウスを離した位置を記録
            endPos = Input.mousePosition;
            isRotating = false;

            // マウスの移動差分を計算
            Vector3 direction = endPos - startPos;

            // 向きを決定する
            SetUnitRotation(direction);
        }
    }
    private void SetUnitRotation(Vector3 direction)
    {
        string facingDirection;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            // 左右の方向を判定
            if (direction.x > 0)
            {
                selectedUnit.transform.rotation = Quaternion.Euler(0, 90, 0); // 右向き

                facingDirection = "右";
            }
            else
            {
                selectedUnit.transform.rotation = Quaternion.Euler(0, -90, 0); // 左向き

                facingDirection = "左";
            }
        }
        else
        {
            // 上下の方向を判定
            if (direction.y > 0)
            {
                selectedUnit.transform.rotation = Quaternion.Euler(0, 0, 0); // 上向き

                facingDirection = "上";
            }
            else
            {
                selectedUnit.transform.rotation = Quaternion.Euler(0, -180, 0); // 下向き


                facingDirection = "下";
            }
        }

        Debug.Log($"ユニットが {facingDirection} を向きました。");
    }

}
