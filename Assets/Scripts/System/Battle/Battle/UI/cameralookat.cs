using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameralookat : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // �������g�̈ʒu����̃I�t�Z�b�g
        Vector3 offset = new Vector3(0, 20, -30);

        // �I�t�Z�b�g�ʒu���v�Z
        Vector3 targetPosition = transform.position + offset;

        // �v�Z�����ʒu������
        transform.LookAt(targetPosition);
    }
}
