using System.Collections.Generic;
using UnityEngine;

public class PowerUPPERitem: MonoBehaviour
{

    public float effect_time;
    public float effect_powerup_percent;
    // ����ς݃I�u�W�F�N�g���i�[���郊�X�g
    public List<GameObject> triggeredObjects = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        // "Player"�^�O�����I�u�W�F�N�g�����m
        if (other.CompareTag("Player") || other.CompareTag("PlayerWall"))
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
        if (other.CompareTag("Player") || other.CompareTag("PlayerWall"))
        {
            // ���łɔ��肳�ꂽ�I�u�W�F�N�g���ǂ������m�F
            if (!triggeredObjects.Contains(other.gameObject))
            {
                triggeredObjects.Add(other.gameObject); // �I�u�W�F�N�g�����X�g�ɒǉ�
                ExecuteFunction(other.gameObject); // �֐������s
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // "Player"�^�O�����I�u�W�F�N�g���͈͊O�ɏo���Ƃ��Ƀ��X�g����폜
        if (other.CompareTag("Player") || other.CompareTag("PlayerWall"))
        {
            if (triggeredObjects.Contains(other.gameObject))
            {
                triggeredObjects.Remove(other.gameObject); // ���X�g����I�u�W�F�N�g���폜
                Debug.Log($"Object exited: {other.gameObject.name}");
            }
        }
    }

    // ����̊֐������s���郁�\�b�h
    private void ExecuteFunction(GameObject playerObject)
    {
        // �����Ɏ��s������������ǉ�
        Debug.LogError($"Function executed for: {playerObject.name}");
        AttackIncreasepercentEffect effect = playerObject.AddComponent<AttackIncreasepercentEffect>();
        effect.ApplyEffect(effect_time, effect_powerup_percent);
    }
}
