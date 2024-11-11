using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player2kill : MonoBehaviour, ISkill
{
    public float cooldownTime = 5f;  // クールダウン時間
    private bool isOnCooldown = false;

    public float effect_time = 10f;
    public float effect_powerup_value = 10f;

    public void Activate()
    {
        if (!isOnCooldown)
        {
            Debug.Log("Skill activated!");

            // スキルの効果をここに実装する
            powerUp();
            // 子オブジェクトにあるAnimatorを取得
            // 子オブジェクトにあるすべてのAnimatorを取得
            Animator[] childAnimators = GetComponentsInChildren<Animator>();

            // 取得したAnimatorごとに処理
            foreach (Animator animator in childAnimators)
            {
                if (animator != null)
                {
                    // 'Attack'トリガーをtrueにする
                    animator.SetTrigger("Attack");
                    Debug.Log("'Attack' トリガーが設定されました！");
                }
            }

            // Animatorが見つからなかった場合
            if (childAnimators.Length == 0)
            {
                Debug.LogWarning("子オブジェクトにAnimatorが見つかりませんでした。");
            }
            else
            {
                Debug.LogWarning("子オブジェクト 'Clicked' にAnimatorが見つかりません。");
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
