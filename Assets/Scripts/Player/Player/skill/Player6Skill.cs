using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player6kill : MonoBehaviour, ISkill
{
    public float cooldownTime = 20f;  // クールダウン時間
    private bool isOnCooldown = false;
    public int cureHP = 10;



    public void Activate()
    {
        if (!isOnCooldown)
        {
            Debug.Log("Skill activated!");

            // スキルの効果をここに実装する
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
                    if (hitCollider.CompareTag("Player"))
                    {
                        //ここに回復要素を追加
                        //回復用スクリプトを作成。（持続回復と一回回復の作成をする）
                        hitCollider.GetComponent<PlayerBattle>().current_HP += cureHP;
                    }
                }

            }
        }
    }
}
