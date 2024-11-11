using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player5kill : MonoBehaviour, ISkill
{
    public float cooldownTime = 20f;  // �N�[���_�E������
    private bool isOnCooldown = false;
    public float stuntime = 10f;


    public GameObject enemy;

    public void Activate()
    {
        if (!isOnCooldown)
        {
            Debug.Log("Skill activated!");

            // �X�L���̌��ʂ������Ɏ�������
            Give_stun();

            StartCoroutine(CooldownRoutine());
        }
        else
        {
            Debug.Log("Skill is on cooldown.");
        }
    }

    public bool IsOnCooldown()
    {
        return isOnCooldown;
    }

    public float GetCooldownTime()
    {
        return cooldownTime;
    }

    private IEnumerator CooldownRoutine()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        isOnCooldown = false;
        Debug.Log("Skill is ready again.");
    }

    public void Give_stun()
    {
        // �q�I�u�W�F�N�g�ɂ��邷�ׂẴR���C�_�[���擾
        Collider[] childColliders = GetComponentsInChildren<Collider>();

        foreach (Collider childCollider in childColliders)
        {
            // isTrigger��true�̃R���C�_�[�ɂ̂ݏ���
            if (childCollider.isTrigger)
            {
                // �g���K�[���̃I�u�W�F�N�g���擾
                Collider[] hitColliders = Physics.OverlapBox(childCollider.bounds.center, childCollider.bounds.extents, childCollider.transform.rotation);
                foreach (Collider hitCollider in hitColliders)
                {
                    // Enemy�^�O�����Ă���I�u�W�F�N�g�ɍU��
                    if (hitCollider.CompareTag("Enemy"))
                    {
                        hitCollider.AddComponent<StunEffect>().ApplyEffect(stuntime,1);
                    }
                }

            }
        }
    }
}