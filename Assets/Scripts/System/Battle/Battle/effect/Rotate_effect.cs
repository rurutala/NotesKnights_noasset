using UnityEngine;

public class Rotate_effect : MonoBehaviour
{
    public float speed = 10f; // ��]���x
    public bool clockwise = true; // ���v��肩�ǂ��������߂�t���O

    public bool rotateX = false; // x���ŉ�]���邩�ǂ���
    public bool rotateY = false; // y���ŉ�]���邩�ǂ���
    public bool rotateZ = false; // z���ŉ�]���邩�ǂ���

    void Update()
    {
        // ��]���������� (clockwise: ���v���A�����v���)
        float rotationDirection = clockwise ? 1f : -1f;

        // ��]�ʂ��v�Z���邽�߂̃x�N�g�����쐬
        Vector3 rotationVector = Vector3.zero;

        // �e���ɑ΂��ĉ�]���邩�ǂ������`�F�b�N
        if (rotateX)
        {
            rotationVector.x = rotationDirection;
        }
        if (rotateY)
        {
            rotationVector.y = rotationDirection;
        }
        if (rotateZ)
        {
            rotationVector.z = rotationDirection;
        }

        // ��]������
        transform.Rotate(rotationVector * speed * Time.deltaTime);
    }
}
