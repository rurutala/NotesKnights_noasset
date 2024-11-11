using System.Collections.Generic;
using UnityEngine;

public class BlockUPitem : MonoBehaviour
{

    public float effect_time;
    public float effect_blockup;
    // ����ς݃I�u�W�F�N�g���i�[���郊�X�g
    private List<GameObject> triggeredObjects = new List<GameObject>();

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
        Debug.Log($"Function executed for: {playerObject.name}");
        BlockIncreaseEffect effect = playerObject.AddComponent<BlockIncreaseEffect>();
        effect.ApplyEffect(effect_time, effect_blockup);
    }
}