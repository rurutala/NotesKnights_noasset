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
        // �}�E�X�̈ʒu���L�^
        startPos = Input.mousePosition;
        isRotating = true;

        // ���ݑI������Ă��郆�j�b�g���擾
        selectedUnit = FindObjectOfType<UnitSelectionSystem>().GetSelectedUnit();
    }

    public void EndRotation()
    {
        if (isRotating && selectedUnit != null)
        {
            // �}�E�X�𗣂����ʒu���L�^
            endPos = Input.mousePosition;
            isRotating = false;

            // �}�E�X�̈ړ��������v�Z
            Vector3 direction = endPos - startPos;

            // ���������肷��
            SetUnitRotation(direction);
        }
    }
    private void SetUnitRotation(Vector3 direction)
    {
        string facingDirection;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            // ���E�̕����𔻒�
            if (direction.x > 0)
            {
                selectedUnit.transform.rotation = Quaternion.Euler(0, 90, 0); // �E����

                facingDirection = "�E";
            }
            else
            {
                selectedUnit.transform.rotation = Quaternion.Euler(0, -90, 0); // ������

                facingDirection = "��";
            }
        }
        else
        {
            // �㉺�̕����𔻒�
            if (direction.y > 0)
            {
                selectedUnit.transform.rotation = Quaternion.Euler(0, 0, 0); // �����

                facingDirection = "��";
            }
            else
            {
                selectedUnit.transform.rotation = Quaternion.Euler(0, -180, 0); // ������


                facingDirection = "��";
            }
        }

        Debug.Log($"���j�b�g�� {facingDirection} �������܂����B");
    }

}
