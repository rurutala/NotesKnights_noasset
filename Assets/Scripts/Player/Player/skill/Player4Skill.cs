using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player4kill : MonoBehaviour, ISkill
{
    public float cooldownTime = 20f;  // クールダウン時間
    private bool isOnCooldown = false;
    public float impactForce = 10f;


    public GameObject enemy;

    public void Activate()
    {
        if (!isOnCooldown)
        {
            Debug.Log("Skill activated!");

            // スキルの効果をここに実装する
            Blow_off();

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

    public void Blow_off()
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

                        Rigidbody rigidbody = hitCollider.GetComponent<Rigidbody>();
                        // スクリプトがついているオブジェクトの現在の向き（forward方向）
                        Vector3 forceDirection = transform.forward;

                        // その方向にimpactForceの大きさの力を加える
                        rigidbody.AddForce(forceDirection * impactForce, ForceMode.Impulse);
                    }
                }

            }
        }
    }
}
