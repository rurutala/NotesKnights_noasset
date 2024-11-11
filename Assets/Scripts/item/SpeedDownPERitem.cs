using System.Collections.Generic;
using UnityEngine;

public class SpeedDownPERitem : MonoBehaviour
{

    public float effect_time;
    public float effect_speeddown_percent_value;
    // ����ς݃I�u�W�F�N�g���i�[���郊�X�g
    private List<GameObject> triggeredObjects = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        // "Player"�^�O�����I�u�W�F�N�g�����m
        if (other.CompareTag("Enemy"))
        {
            // ���łɔ��肳�ꂽ�I�u�W�F�N�g���ǂ������m�F
            if (!triggeredObjects.Contains(other.gameObject))
            {
                triggeredObjects.Add(other.gameObject); // �I�u�W�F�N�g�����X�g�ɒǉ�
                ExecuteFunction(other.gameObject); // �֐������s
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // "Player"�^�O�����I�u�W�F�N�g�����m
        if (other.CompareTag("Enemy"))
        {
            // ���łɔ��肳�ꂽ�I�u�W�F�N�g���ǂ������m�F
            if (!triggeredObjects.Contains(other.gameObject))
            {
                triggeredObjects.Add(other.gameObject); // �I�u�W�F�N�g�����X�g�ɒǉ�
                ExecuteFunction(other.gameObject); // �֐������s
            }
        }
    }

    // ����̊֐������s���郁�\�b�h
    private void ExecuteFunction(GameObject enemyObject)
    {
        // �����Ɏ��s������������ǉ�
        Debug.Log($"Function executed for: {enemyObject.name}");
        MoveDecreasepercentEffect effect = enemyObject.AddComponent<MoveDecreasepercentEffect>();
        effect.ApplyEffect(effect_time, effect_speeddown_percent_value);
    }
}
