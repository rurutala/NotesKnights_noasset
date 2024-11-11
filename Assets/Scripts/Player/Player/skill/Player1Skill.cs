using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1Skill : MonoBehaviour, ISkill
{
    public float cooldownTime = 5f;  // クールダウン時間
    private bool isOnCooldown = false;
    public int attackDamage = 10; // 攻撃力

    public void Activate()
    {
        if (!isOnCooldown)
        {
            Debug.Log("Skill activated!");

            // スキルの効果をここに実装する
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

    // 子オブジェクトのトリガーに触れている敵に攻撃する処理
    private void AttackEnemiesInTrigger()
    {
        // 子オブジェクトにあるすべてのコライダーを取得
        Collider[] childColliders = GetComponentsInChildren<Collider>();

        foreach (Collider childCollider in childColliders)
        {
            // isTriggerがtrueのコライダーにのみ処理
            if (childCollider.isTrigger)
            {
                // トリガー内のオブジェクトを取得
                Collider[] hitColliders = Physics.OverlapBox(childCollider.bounds.center, childCollider.bounds.extents, childCollider.transform.rotation);

                foreach (Collider hitCollider in hitColliders)
                {
                    // Enemyタグがついているオブジェクトに攻撃
                    if (hitCollider.CompareTag("Enemy"))
                    {
                        Debug.Log("Attacking enemy: " + hitCollider.name);
                        EnemyBattle enemy = hitCollider.GetComponent<EnemyBattle>();

                        if (enemy != null)
                        {
                            enemy.TakeDamage(attackDamage); // 攻撃ダメージを与える
                        }
                    }
                }
            }
        }
    }
}
