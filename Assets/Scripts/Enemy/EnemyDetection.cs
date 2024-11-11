using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    // ���łɎQ�Ƃ����I�u�W�F�N�g��ێ����邽�߂�HashSet
    private HashSet<GameObject> detectedEnemies = new HashSet<GameObject>();

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
        // "Enemy"�^�O���t�����I�u�W�F�N�g���m�F
        if (other.CompareTag("Enemy"))
        {
            // ���ɎQ�Ƃ��Ă��邩�ǂ������m�F
            if (!detectedEnemies.Contains(other.gameObject))
            {
                // ���߂ĎQ�Ƃ��ꂽ�I�u�W�F�N�g�̏ꍇ�AHashSet�ɒǉ����ď������s��
                detectedEnemies.Add(other.gameObject);
                HandleEnemyDetected(other.gameObject);
            }
        }
    }

    // �G�����o���ꂽ���̏���
    private void HandleEnemyDetected(GameObject enemy)
    {
        // �����ɓG�����o���ꂽ���̏���������
        Debug.Log($"Enemy detected: {enemy.name}");
        GameFlowManager.Instance.EnemyInvasion();

    }
}
