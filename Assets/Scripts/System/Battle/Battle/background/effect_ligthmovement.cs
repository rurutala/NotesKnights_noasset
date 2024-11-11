using UnityEngine;

public class PingPongMovement : MonoBehaviour
{
    public float maxX = 10f; // x���W�̍ŏ��̒n�_����̍ő勗��
    public float minX = -10f; // x���W�̍ŏ��̒n�_����̍ŏ�����
    public float maxZ = 10f; // z���W�̍ŏ��̒n�_����̍ő勗��
    public float minZ = -10f; // z���W�̍ŏ��̒n�_����̍ŏ�����
    public float speed = 1f; // �ړ����x

    public bool useX = true; // x�����g�p���邩�ǂ���
    public bool useZ = false; // z�����g�p���邩�ǂ���

    private Vector3 initialPosition; // �I�u�W�F�N�g�̏����ʒu
    private bool movingPositive = true; // �������i�E�܂��͏�j�Ɉړ������������t���O

    void Start()
    {
        // �I�u�W�F�N�g�̏����ʒu���L�^
        initialPosition = transform.position;
    }

    void Update()
    {
        // ���݂̈ʒu���擾
        Vector3 currentPosition = transform.position;

        // x���̈ړ�
        if (useX)
        {
            if (movingPositive)
            {
                currentPosition.x += speed * Time.deltaTime; // �E�Ɉړ�
                if (currentPosition.x >= initialPosition.x + maxX)
                {
                    movingPositive = false; // �ő�l�ɒB�����獶�Ɉړ�
                }
            }
            else
            {
                currentPosition.x -= speed * Time.deltaTime; // ���Ɉړ�
                if (currentPosition.x <= initialPosition.x + minX)
                {
                    movingPositive = true; // �ŏ��l�ɒB������E�Ɉړ�
                }
            }
        }

        // z���̈ړ�
        if (useZ)
        {
            if (movingPositive)
            {
                currentPosition.z += speed * Time.deltaTime; // ��Ɉړ�
                if (currentPosition.z >= initialPosition.z + maxZ)
                {
                    movingPositive = false; // �ő�l�ɒB�����牺�Ɉړ�
                }
            }
            else
            {
                currentPosition.z -= speed * Time.deltaTime; // ���Ɉړ�
                if (currentPosition.z <= initialPosition.z + minZ)
                {
                    movingPositive = true; // �ŏ��l�ɒB�������Ɉړ�
                }
            }
        }

        // �V�����ʒu�ɃI�u�W�F�N�g���ړ�
        transform.position = currentPosition;
    }
}
