using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player2kill : MonoBehaviour, ISkill
{
    public float cooldownTime = 5f;  // �N�[���_�E������
    private bool isOnCooldown = false;

    public float effect_time = 10f;
    public float effect_powerup_value = 10f;

    public void Activate()
    {
        if (!isOnCooldown)
        {
            Debug.Log("Skill activated!");

            // �X�L���̌��ʂ������Ɏ�������
            powerUp();
            // �q�I�u�W�F�N�g�ɂ���Animator���擾
            // �q�I�u�W�F�N�g�ɂ��邷�ׂĂ�Animator���擾
            Animator[] childAnimators = GetComponentsInChildren<Animator>();

            // �擾����Animator���Ƃɏ���
            foreach (Animator animator in childAnimators)
            {
                if (animator != null)
                {
                    // 'Attack'�g���K�[��true�ɂ���
                    animator.SetTrigger("Attack");
                    Debug.Log("'Attack' �g���K�[���ݒ肳��܂����I");
                }
            }

            // Animator��������Ȃ������ꍇ
            if (childAnimators.Length == 0)
            {
                Debug.LogWarning("�q�I�u�W�F�N�g��Animator��������܂���ł����B");
            }
            else
            {
                Debug.LogWarning("�q�I�u�W�F�N�g 'Clicked' ��Animator��������܂���B");
            }
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


    private void powerUp()
    {
        AttackIncreaseEffect effect = this.gameObject.AddComponent<AttackIncreaseEffect>();
        effect.ApplyEffect(effect_time,effect_powerup_value);
    }
}
