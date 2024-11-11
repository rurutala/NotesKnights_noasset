using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stage_holl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // �Ԃ������I�u�W�F�N�g��Enemy�^�O�������Ă��邩�m�F
        if (other.CompareTag("Enemy"))
        {
            // �Ԃ������I�u�W�F�N�g��EnemyBattle�X�N���v�g���擾
            EnemyBattle enemyBattle = other.GetComponent<EnemyBattle>();

            // EnemyBattle�X�N���v�g������������Die���\�b�h���Ăяo��
            if (enemyBattle != null)
            {
                enemyBattle.Die();
                Debug.Log("Enemy died.");
            }
            else
            {
                Debug.LogError("EnemyBattle script not found on object with Enemy tag.");
            }
        }
    }
}
