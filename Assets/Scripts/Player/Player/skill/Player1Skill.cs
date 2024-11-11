using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1Skill : MonoBehaviour, ISkill
{
    public float cooldownTime = 5f;  // �N�[���_�E������
    private bool isOnCooldown = false;
    public int attackDamage = 10; // �U����

    public void Activate()
    {
        if (!isOnCooldown)
        {
            Debug.Log("Skill activated!");

            // �X�L���̌��ʂ������Ɏ�������
            AttackEnemiesInTrigger();

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

    // �q�I�u�W�F�N�g�̃g���K�[�ɐG��Ă���G�ɍU�����鏈��
    private void AttackEnemiesInTrigger()
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
                        Debug.Log("Attacking enemy: " + hitCollider.name);
                        EnemyBattle enemy = hitCollider.GetComponent<EnemyBattle>();

                        if (enemy != null)
                        {
                            enemy.TakeDamage(attackDamage); // �U���_���[�W��^����
                        }
                    }
                }
            }
        }
    }
}
